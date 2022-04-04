using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulgarianPlacesAPI.Models
{
    public class Place
    {
        public int Id { get; set; }
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public string Image { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual User CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(11, 8)")]
        public decimal? Latitude { get; set; }
        [Column(TypeName = "decimal(11, 8)")]
        public decimal? Longitude { get; set; }
    }
}
