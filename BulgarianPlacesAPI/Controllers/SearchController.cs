using BulgarianPlacesAPI.Dtos;
using BulgarianPlacesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BulgarianPlacesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IPlaceService placeService;

        public SearchController(IUserService userService,
            IPlaceService placeService)
        {
            this.userService = userService;
            this.placeService = placeService;
        }

        [HttpGet("{text}")]
        public IActionResult PerformSearch(string text)
        {
            var toReturn = new List<SearchDto>();
            toReturn.AddRange(this.userService.SearchUser(text));
            toReturn.AddRange(this.placeService.SearchPlaces(text));

            toReturn.OrderBy(x => Guid.NewGuid());
            return Ok(toReturn);
        }
    }
}
