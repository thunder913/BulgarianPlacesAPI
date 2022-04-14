using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulgarianPlacesAPI.Models
{
    public class AdminRequest
    {
        public int Id { get; set; }
        public int? PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string JwtToken { get; set; }
        public int UserId { get; set; }
    }
}
