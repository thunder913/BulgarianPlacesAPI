using BulgarianPlacesAPI.Dtos;
using BulgarianPlacesAPI.Models;
using BulgarianPlacesAPI.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BulgarianPlacesAPI.Services.Interfaces
{
    public interface IReviewService
    {
        Task<int> AddReviewAsync(Review review, string bae64Image);
        PlaceReviewDto GetReviewById(int id);
        List<AdminPanelDto> GetAdminPanelItems();
        AdminApprovalDto GetReviewToApproveById(int id);
        Task ChangeReviewStatusAsync(AdminRequest request, ReviewStatus status);
    }
}
