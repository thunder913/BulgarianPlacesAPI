namespace BulgarianPlacesAPI.Dtos
{
    public class RankingUserDto
    {
        public int Id { get; set; }
        public int Rank{ get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int PlacesVisited { get; set; }
    }
}
