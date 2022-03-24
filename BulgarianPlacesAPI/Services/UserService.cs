using BulgarianPlacesAPI.Data;
using BulgarianPlacesAPI.Dtos;
using BulgarianPlacesAPI.Models;
using BulgarianPlacesAPI.Models.Enums;
using BulgarianPlacesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulgarianPlacesAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public User GetByEmail(string email)
        {
            return this.dbContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public User GetById(int id)
        {
            return this.dbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public ProfileDto GetProfileById(int id)
        {
            return this.dbContext
                .Users
                .Include(x => x.Reviews)
                .ThenInclude(x => x.Place)
                .Where(x => x.Id == id)
                .Select(x => new ProfileDto()
                {
                    Description = x.Description,
                    Image = x.Image,
                    Name = x.FirstName + " " + x.LastName,
                    Visited = x.Reviews.Count(y => y.Status == ReviewStatus.Approved),
                    VisitedLastMonth = x.Reviews.Where(y => y.Status == ReviewStatus.Approved && y.DateCreated >= DateTime.UtcNow.AddMonths(-1)).Count(),
                    PlacesVisited = x.Reviews.Where(y => y.Status == ReviewStatus.Approved).Select(y => new ProfilePlaceVisited() 
                    {
                        Date = y.DateCreated.ToString("dd/MM/yyyy"),
                        Id = y.Id,
                        PlaceName = y.Place.Name,
                        Rating = y.Rating,
                    }).ToList(),
                })
                .FirstOrDefault();
        }

        public List<RankingUserDto> GetUserRanking(RankingType rankingType)
        {
            var users = this.dbContext.Users.Include(x => x.Reviews).Where(x => x.HasCompletedFirstTime).AsQueryable();
            var toReturn = new List<RankingUserDto>();
            var date = DateTime.UtcNow;
            switch (rankingType)
            {
                case RankingType.Weekly:
                    if (date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        date = date.AddDays(-6);
                    }
                    else
                    {
                        date = date.AddDays(DayOfWeek.Monday - date.DayOfWeek);
                    }

                    toReturn = users.Select(x => new RankingUserDto()
                    {
                        Id = x.Id,
                        Image = x.Image,
                        Name = x.FirstName + " " + x.LastName,
                        PlacesVisited = x.Reviews.Count(y => y.DateCreated >= date && y.Status == ReviewStatus.Approved),
                    })
                    .OrderByDescending(x => x.PlacesVisited)
                    .Take(100)
                    .ToList();

                    break;
                case RankingType.Monthly:
                    toReturn = users.Select(x => new RankingUserDto()
                    {
                        Id = x.Id,
                        Image = x.Image,
                        Name = x.FirstName + " " + x.LastName,
                        PlacesVisited = x.Reviews.Count(y => y.DateCreated.Month >= DateTime.UtcNow.Month && y.Status == ReviewStatus.Approved),
                    })
                    .OrderByDescending(x => x.PlacesVisited)
                    .Take(100)
                    .ToList();
                    break;
                case RankingType.Yearly:
                    toReturn = users.Select(x => new RankingUserDto()
                    {
                        Id = x.Id,
                        Image = x.Image,
                        Name = x.FirstName + " " + x.LastName,
                        PlacesVisited = x.Reviews.Count(y => y.DateCreated.Year == DateTime.UtcNow.Year && y.Status == ReviewStatus.Approved),
                    })
                    .OrderByDescending(x => x.PlacesVisited)
                    .Take(100)
                    .ToList();
                    break;
                case RankingType.AllTime:
                    toReturn = users.Select(x => new RankingUserDto()
                    {
                        Id = x.Id,
                        Image = x.Image,
                        Name = x.FirstName + " " + x.LastName,
                        PlacesVisited = x.Reviews.Count(y => y.Status == ReviewStatus.Approved),
                    })
                    .OrderByDescending(x => x.PlacesVisited)
                    .Take(100)
                    .ToList();
                    break;
            }

            for (int i = 0; i < toReturn.Count(); i++)
            {
                toReturn[i].Rank = i + 1;
            }

            return toReturn;
        }

        public async Task<int> AddUserAsync(User user)
        {
            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();
            return user.Id;
        }

        public async Task FinishFirstTimePopUpAsync(int id, string firstName, string lastName, string image, string description)
        {
            var user = this.dbContext.Users.FirstOrDefault(x => x.Id == id);

            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName) && !string.IsNullOrWhiteSpace(image) && !string.IsNullOrWhiteSpace(description))
            {
                throw new Exception("User already has filled properties!");
            }

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Image = image;
            user.Description = description;
            user.HasCompletedFirstTime = true;
            await this.dbContext.SaveChangesAsync();
        }

        public List<SearchDto> SearchUser(string text)
        {
            return this.dbContext
                .Users
                .Include(x => x.Reviews)
                .Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(text))
                .Select(x => new SearchDto()
                {
                    Id = x.Id,
                    Image = x.Image,
                    LastColumnValue = x.Reviews.Count(y => y.Status == ReviewStatus.Approved),
                    Name = x.FirstName + " " + x.LastName,
                    SearchType = SearchResultType.Person
                })
                .ToList();
        }
    }
}
