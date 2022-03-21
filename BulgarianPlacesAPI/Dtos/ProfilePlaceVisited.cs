using System;

namespace BulgarianPlacesAPI.Dtos
{
    public class ProfilePlaceVisited
    {
        public string PlaceName { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }
    }
}
