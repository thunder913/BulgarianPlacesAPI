using BulgarianPlacesAPI.Models;
using BulgarianPlacesAPI.Models.Enums;
using BulgarianPlacesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BulgarianPlacesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddReview(string image, int rating, string description, decimal chosenLatitude, 
            decimal chosenLongitude, bool isAtLocation, decimal userLatitude, decimal userLongitude, int placeId, int userId)
        {
            //TODO save image to azure or something and then save it as a link in the DB

            var review = new Review()
            {
                Image = image,
                Rating = rating,
                Description = description,
                PlaceLatitude = chosenLatitude,
                PlaceLongitude = chosenLongitude,
                IsAtLocation = isAtLocation,
                UserLatitude = userLatitude,
                UserLongitude = userLongitude,
                Status = ReviewStatus.Submitted,
                DateCreated = DateTime.UtcNow,
                PlaceId = placeId,
                UserId = userId
            };

            return Ok(await this.reviewService.AddReviewAsync(review));
        }

        [HttpGet("{id}")]
        public IActionResult GetReviewById(int id)
        {
            return Ok(this.reviewService.GetReviewById(id));
        }

        [HttpGet("admin")]
        public IActionResult GetReviewsToApprove()
        {
            return Ok(this.reviewService.GetAdminPanelItems());
        }

        [HttpGet("adminreview/{id}")]
        public IActionResult GetAdminReview(int id)
        {
            return Ok(this.reviewService.GetReviewToApproveById(id));
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveReview(int id)
        {
            try
            {
                await this.reviewService.ChangeReviewStatusAsync(id, ReviewStatus.Approved);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("decline/{id}")]
        public async Task<IActionResult> DeclineReview(int id)
        {
            try
            {
                await this.reviewService.ChangeReviewStatusAsync(id, ReviewStatus.Declined);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
