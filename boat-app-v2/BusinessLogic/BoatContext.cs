using boat_app_v2.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace boat_app_v2.BusinessLogic
{
    public class BoatContext : DbContext
    {
        public BoatContext(DbContextOptions options) 
            : base(options) 
        { 
            //this.SeedData();
        }

        public DbSet<Boat>? Boats { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Boat>()
                .HasKey(b => b.Code)
                .HasName("PrimaryKey_CodeId");
        }
    }
}