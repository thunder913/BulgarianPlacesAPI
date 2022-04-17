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

        public PlaceService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
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
                    Image = x.Image,
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
                    Image = x.Image,
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
