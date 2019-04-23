using CityExplorer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityExplorer.Data
{
    public class CityContext : DbContext
    {
        private readonly IConfiguration config;

        public DbSet<City> Cities { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Venue> Venues { get; set; }

        public CityContext(DbContextOptions options, IConfiguration config): base(options)
        {
            this.config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(config.GetConnectionString("CityExplorer"));
        }

        protected override void OnModelCreating(ModelBuilder bldr)
        {
            bldr.Entity<City>()
                .HasData(new
                {
                    CityId = 1,
                    Name = "mtl"
                });

            bldr.Entity<City>()
                .HasData(new
                {
                    CityId = 2,
                    Name = "van",
                });

            bldr.Entity<Activity>()
                .HasData(new
                {
                    ActivityId = 1,
                    Name = "chilling"
                });

            bldr.Entity<Venue>()
                .HasData(new
                {
                    VenueId = 1,
                    Name = "Brewtopia",
                    Type = "Bar",
                    Address = "1219 Crescent St, Montreal, QC H3G 2B1",
                    Affordability = 1
                });

            bldr.Entity<ActivityCity>()
                .HasKey(x => new { x.CityId, x.ActivityId });

            bldr.Entity<ActivityVenue>()
                .HasKey(x => new { x.VenueId, x.ActivityId });
        }
    }
}
