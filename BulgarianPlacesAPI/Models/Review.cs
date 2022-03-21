using BulgarianPlacesAPI.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulgarianPlacesAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        [Column(TypeName = "decimal(11, 8)")]
        public decimal? UserLatitude { get; set; }
        [Column(TypeName = "decimal(11, 8)")]
        public decimal? UserLongitude { get; set; }
        [Column(TypeName = "decimal(11, 8)")]
        public decimal? PlaceLatitude { get; set; }
        [Column(TypeName = "decimal(11, 8)")]
        public decimal? PlaceLongitude { get; set; }
        public string Image { get; set; }
        public ReviewStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public virtual Place Place { get; set; }
        public int PlaceId { get; set; }
        public string Description { get; set; }
        public  bool IsAtLocation { get; set; }
    }
}
