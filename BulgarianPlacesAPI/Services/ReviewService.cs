using BlobStorage;
using BulgarianPlacesAPI.Data;
using BulgarianPlacesAPI.Dtos;
using BulgarianPlacesAPI.Models;
using BulgarianPlacesAPI.Models.Enums;
using BulgarianPlacesAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulgarianPlacesAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IBlobService blobService;

        public ReviewService(ApplicationDbContext dbContext, IBlobService blobService)
        {
            this.dbContext = dbContext;
            this.blobService = blobService;
        }
        public async Task<int> AddReviewAsync(Review review, string base64Image)
        {
            var name = Guid.NewGuid().ToString() + ".jpeg";
            await blobService.UploadBase64StringAsync(base64Image, name, "images");
            review.Image = name;
            await this.dbContext.AddAsync(review);
            await this.dbContext.SaveChangesAsync();
            return review.Id;
        }

        public PlaceReviewDto GetReviewById(int id)
        {
            return this.dbContext
                .Reviews
                .Include(x => x.User)
                .Where(x => x.Id == id)
                .Select(x => new PlaceReviewDto()
                {
                    Id = x.Id,
                    Image = x.Image,
                    Date = x.DateCreated.ToString("dd/MM/yyyy"),
                    Comment = x.Description,
                    Creator = "by " + x.User.FirstName + " " + x.User.LastName,
                    Rating = x.Rating
                })
                .FirstOrDefault();
        }

        public List<AdminPanelDto> GetAdminPanelItems()
        {
            return this.dbContext
                .Reviews
                .Include(x => x.User)
                .Where(x => x.Status == ReviewStatus.Submitted)
                .Select(x => new AdminPanelDto()
                {
                    DateSubmitted = x.DateCreated.ToString("dd/MM/yyyy"),
                    Email = x.User.Email,
                    Id = x.Id,
                })
                .ToList();
        }

        public AdminApprovalDto GetReviewToApproveById(int id)
        {
            return this.dbContext
                .Reviews
                .Include(x => x.User)
                .Where(x => x.Id == id)
                .Select(x => new AdminApprovalDto()
                {
                    Id = x.Id,
                    Checkbox = x.IsAtLocation,
                    Date = x.DateCreated.ToString("dd/MM/yyyy"),
                    Description = x.Description,
                    Email = x.User.Email,
                    Image = this.blobService.GetBlobUrlAsync(x.Image, "images").GetAwaiter().GetResult(),
                    LocationLatitude = x.PlaceLatitude,
                    LocationLongitude = x.PlaceLongitude,
                    Name = x.User.FirstName + " " + x.User.LastName,
                    UserLatitude = x.UserLatitude,
                    UserLongitude = x.UserLongitude,
                })
                .FirstOrDefault();
        }

        public async Task ChangeReviewStatusAsync(AdminRequest request, ReviewStatus status)
        {
            var review = this.dbContext
                .Reviews
                .FirstOrDefault(x => x.Id == request.Id);
            if (status != ReviewStatus.Declined)
            {
                int? placeId = request.PlaceId ?? null;
                if (placeId is null)
                {
                    var place = this.dbContext.Places.Add(new Place()
                    {
                        CreatedById = request.UserId,
                        CreatedOn = DateTime.UtcNow,
                        Latitude = review.PlaceLatitude,
                        Longitude = review.PlaceLongitude,
                        Name = request.PlaceName,
                        Image = review.Image,
                    });


                    review.Place = place.Entity;
                }
                else
                {
                    review.PlaceId = request.PlaceId;
                }
            }

            review.Status = status;
            review.DateModified = DateTime.UtcNow;
            await this.dbContext.SaveChangesAsync();
        }
    }
}
