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

        public ReviewService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddReviewAsync(Review review)
        {
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
                    Checkbox = x.IsAtLocation,
                    Date = x.DateCreated.ToString("dd/MM/yyyy"),
                    Description = x.Description,
                    Email = x.User.Email,
                    Image = x.Image,
                    LocationLatitude = x.PlaceLatitude,
                    LocationLongitude = x.PlaceLongitude,
                    Name = x.User.FirstName + " " + x.User.LastName,
                    UserLatitude = x.UserLatitude,
                    UserLongitude = x.UserLongitude,
                })
                .FirstOrDefault();
        }

        public async Task ChangeReviewStatusAsync(int id, ReviewStatus status)
        {
            var review = this.dbContext
                .Reviews
                .FirstOrDefault(x => x.Id == id);

            review.Status = status;
            review.DateModified = DateTime.UtcNow;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
