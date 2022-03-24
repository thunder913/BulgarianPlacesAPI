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
        private readonly IUserService userService;
        private readonly IJwtService jwtService;

        public ReviewController(IReviewService reviewService, IUserService userService, IJwtService jwtService)
        {
            this.reviewService = reviewService;
            this.userService = userService;
            this.jwtService = jwtService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddReview(string image, int rating, string description, decimal chosenLatitude, 
            decimal chosenLongitude, bool isAtLocation, decimal userLatitude, decimal userLongitude, int placeId, string jwt)
        {
            //TODO save image to azure or something and then save it as a link in the DB
            try
            {
                var user = this.GetUserByToken(jwt);
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
                    UserId = user.Id
                };

                return Ok(await this.reviewService.AddReviewAsync(review));
            }
            catch (Exception)
            {
                return BadRequest("Your token is invalid!");
            }
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

        protected User GetUserByToken(string token)
        {
            if (token == null)
            {
                return null;
            }
            var verify = this.jwtService.Verify(token);

            var userId = verify.Id;

            return this.userService.GetById(int.Parse(userId));
        }
    }
}
