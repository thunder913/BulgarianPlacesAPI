using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulgarianPlacesAPI.Dtos
{
    public class FinishFirstTimeRequest
    {
        public string Jwt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}
