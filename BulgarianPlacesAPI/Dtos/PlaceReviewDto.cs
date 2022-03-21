namespace BulgarianPlacesAPI.Dtos
{
    public class PlaceReviewDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Date { get; set; }
        public string Creator { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
