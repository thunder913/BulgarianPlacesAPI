using BulgarianPlacesAPI.Models.Enums;
using System.Collections.Generic;

namespace BulgarianPlacesAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public UserType UserType { get; set; } = UserType.User;
        public bool HasCompletedFirstTime { get; set; }
    }
}
