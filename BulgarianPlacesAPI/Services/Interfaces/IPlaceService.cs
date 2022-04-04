using BulgarianPlacesAPI.Dtos;
using System.Collections.Generic;

namespace BulgarianPlacesAPI.Services.Interfaces
{
    public interface IPlaceService
    {
        List<SearchDto> SearchPlaces(string text);
        PlaceDto GetPlaceById(int id);
    }
}
