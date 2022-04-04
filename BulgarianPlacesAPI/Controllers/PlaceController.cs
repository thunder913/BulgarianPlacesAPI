using BulgarianPlacesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BulgarianPlacesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService placeService;

        public PlaceController(IPlaceService placeService)
        {
            this.placeService = placeService;
        }

        [HttpGet("{id}")]
        public IActionResult GetPlaceById(int id)
        {
            return Ok(this.placeService.GetPlaceById(id));
        }
    }
}
