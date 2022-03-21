using BulgarianPlacesAPI.Dtos;
using BulgarianPlacesAPI.Models;
using BulgarianPlacesAPI.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BulgarianPlacesAPI.Services.Interfaces
{
    public interface IUserService
    {
        User GetByEmail(string email);
        User GetById(int id);
        ProfileDto GetProfileById(int id);
        Task<int> AddUserAsync(User user);
        List<RankingUserDto> GetUserRanking(RankingType rankingType);
        Task FinishFirstTimePopUpAsync(int id, string firstName, string lastName, string image, string description);
        List<SearchDto> SearchUser(string text);
    }
}
