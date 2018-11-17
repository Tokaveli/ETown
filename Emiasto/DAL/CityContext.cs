using System.Data.Entity;

using Microsoft.AspNet.Identity.EntityFramework;

using Emiasto.Models;

namespace Emiasto.DAL
{
    public class CityContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Facility> Facilities { get; set; }

        public DbSet<FacilityImage> FacilityImages { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<ReviewImage> ReviewImages { get; set; }

        public DbSet<ReviewLike> ReviewLikes { get; set; }

        public CityContext() : base("DefaultConnection") { }
    }
}
