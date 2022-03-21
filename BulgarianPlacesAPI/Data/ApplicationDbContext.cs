using BulgarianPlacesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BulgarianPlacesAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .HasOne(x => x.User)
                .WithMany(x => x.Reviews)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
