namespace BulgarianPlacesAPI.Dtos
{
    public class AddReviewProperties
    {
        public string Image { get; set; }
        public string Description { get; set; }
        public int rating { get; set; }
        public decimal chosenLatitude { get; set; }
        public decimal chosenLongitude { get; set; }
        public bool isAtLocation { get; set; }
        public decimal userLatitude { get; set; }
        public decimal userLongitude { get; set; }
        public string jwt { get; set; }
    }
}
