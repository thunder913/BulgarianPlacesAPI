namespace BulgarianPlacesAPI.Dtos
{
    public class AdminApprovalDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Checkbox { get; set; }
        public decimal? LocationLatitude { get; set; }
        public decimal? LocationLongitude { get; set; }
        public decimal? UserLongitude { get; set; }
        public decimal? UserLatitude { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}
