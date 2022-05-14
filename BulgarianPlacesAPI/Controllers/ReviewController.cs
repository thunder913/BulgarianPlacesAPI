using BulgarianPlacesAPI.Dtos;
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
        public async Task<IActionResult> AddReview([FromForm] AddReviewProperties props)
        {
            try
            {
                var user = this.GetUserByToken(props.jwt);
                var review = new Review()
                {
                    Rating = props.rating,
                    Description = props.Description,
                    PlaceLatitude = props.chosenLatitude,
                    PlaceLongitude = props.chosenLongitude,
                    IsAtLocation = props.isAtLocation,
                    UserLatitude = props.userLatitude,
                    UserLongitude = props.userLongitude,
                    Status = ReviewStatus.Submitted,
                    DateCreated = DateTime.UtcNow,
                    UserId = user.Id
                };

                return Ok(await this.reviewService.AddReviewAsync(review, props.Image));
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

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveReview([FromForm] AdminRequest request)
        {
            try
            {
                var user = this.GetUserByToken(request.JwtToken);
                if (request.PlaceId is null && string.IsNullOrWhiteSpace(request.PlaceName))
                {
                    return BadRequest("Провери, дали си избрал мястото, където се намира ревюто.");
                }
                else if (user.UserType != UserType.Admin)
                {
                    return BadRequest("Нямаш права, да извършваш тази дейност!");
                }
                request.UserId = user.Id;
                await this.reviewService.ChangeReviewStatusAsync(request, ReviewStatus.Approved);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("decline")]
        public async Task<IActionResult> DeclineReview([FromForm] AdminRequest request)
        {
            try
            {
                var user = this.GetUserByToken(request.JwtToken);
                request.UserId = user.Id;
                await this.reviewService.ChangeReviewStatusAsync(request, ReviewStatus.Declined);
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
