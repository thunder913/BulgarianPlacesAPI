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
    public class UserController : ControllerBase
    {
        private readonly IJwtService jwtService;
        private readonly IUserService userService;

        public UserController(IJwtService jwtService,
            IUserService userService)
        {
            this.jwtService = jwtService;
            this.userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login(string email, string password)
        {
            var user = this.userService.GetByEmail(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return BadRequest("Invalid credentials!");
            }

            var jwt = this.jwtService.Generate(user.Id.ToString(), user.Email);

            return Ok(new
            {
                message = "success",
                email = user.Email,
                jwtToken = jwt,
                Id = user.Id,
                IsAdmin = user.UserType == UserType.Admin,
                HasCompletedFirstTime = user.HasCompletedFirstTime,
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string email, string password)
        {
            var user = this.userService.GetByEmail(email);
            if (user != null)
            {
                return BadRequest("User already exists!");
            }

            user = new User()
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
            };

            return Ok(await this.userService.AddUserAsync(user));
        }

        [HttpGet("profile/{id}")]
        public IActionResult GetProfile(int id)
        {
            return Ok(this.userService.GetProfileById(id));
        }

        [HttpGet("ranking/{type}")]
        public IActionResult GetRanking(RankingType type)
        {
            return Ok(this.userService.GetUserRanking(type));
        }

        [HttpPost("FinishFirstTime")]
        public async Task<IActionResult> FinishFirstTimePopUp([FromForm] FinishFirstTimeRequest request)
        {
            try
            {
                var user = this.GetUserByToken(request.Jwt);
                if (user == null)
                {
                    return BadRequest();
                }
                await this.userService.FinishFirstTimePopUpAsync(user.Id, request.FirstName, request.LastName, request.Image, request.Description);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }

        [HttpPost("VerifyToken")]
        public IActionResult VerifyToken(string token)
        {
            try
            {
                var user = this.GetUserByToken(token);
                if (user == null)
                {
                    return BadRequest("Invalid token!");
                }
                return Ok(new { Id = user.Id, IsAdmin = user.UserType == UserType.Admin, HasCompletedFirstTime = user.HasCompletedFirstTime });
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
