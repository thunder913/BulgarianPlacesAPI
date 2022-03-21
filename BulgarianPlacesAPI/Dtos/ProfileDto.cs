using System.Collections.Generic;

namespace BulgarianPlacesAPI.Dtos
{
    public class ProfileDto
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Visited { get; set; }
        public int VisitedLastMonth { get; set; }
        public ICollection<ProfilePlaceVisited> PlacesVisited { get; set; } = new List<ProfilePlaceVisited>();
    }
}
