using BulgarianPlacesAPI.Models.Enums;

namespace BulgarianPlacesAPI.Dtos
{
    public class SearchDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int LastColumnValue { get; set; }
        public SearchResultType SearchType { get; set; }
    }
}
