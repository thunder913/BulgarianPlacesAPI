using BlobStorage;
using BulgarianPlacesAPI.Data;
using BulgarianPlacesAPI.Dtos;
using BulgarianPlacesAPI.Models.Enums;
using BulgarianPlacesAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BulgarianPlacesAPI.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IBlobService blobService;

        public PlaceService(ApplicationDbContext dbContext, IBlobService blobService)
        {
            this.dbContext = dbContext;
            this.blobService = blobService;
        }

        public List<SearchDto> SearchPlaces(string text)
        {
            return this.dbContext
                .Places
                .Include(x => x.Reviews)
                .Where(x => x.Name.ToLower().Contains(text))
                .Select(x => new SearchDto()
                {
                    Id = x.Id,
                    Image = this.blobService.GetBlobUrlAsync(x.Image, "images").GetAwaiter().GetResult(),
                    LastColumnValue = (x.Reviews.Where(y => y.Status == ReviewStatus.Approved).Sum(y => y.Rating))/(x.Reviews.Count(y => y.Status == ReviewStatus.Approved) != 0 ? x.Reviews.Count(y => y.Status == ReviewStatus.Approved) : 1),
                    Name = x.Name,
                    SearchType = SearchResultType.Place,
                })
                .ToList();
        }

        public PlaceDto GetPlaceById(int id)
        {
            return this.dbContext
                .Places
                .Include(x => x.Reviews)
                .Where(x => x.Id == id)
                .Select(x => new PlaceDto()
                {
                    Id = x.Id,
                    Image = this.blobService.GetBlobUrlAsync(x.Image, "images").GetAwaiter().GetResult(),
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Name = x.Name,
                    Rating = (x.Reviews.Where(y => y.Status == ReviewStatus.Approved).Sum(y => y.Rating)) / (x.Reviews.Count(y => y.Status == ReviewStatus.Approved) != 0 ? x.Reviews.Count(y => y.Status == ReviewStatus.Approved) : 1),
                    Visits = x.Reviews.Where(y => y.Status == ReviewStatus.Approved).Count(),
                })
                .SingleOrDefault();
        }
    }
}
