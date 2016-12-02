using GreenHouseWebApi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GreenHouseWebApi.Repository
{
    public class GreenHouseDatabaseContext : DbContext
    {
        public GreenHouseDatabaseContext(DbContextOptions<GreenHouseDatabaseContext> options): base(options)
        {
        }

        // public DbSet<HumidityLogEntry> AirHumidityLogEntries { get; set; }
        // public DbSet<SoilHumidityLogEntry> SoilHumidityLogEntries { get; set; }

        public DbSet<DeviceConfiguration> DeviceConfigurations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceConfiguration>().HasMany<WateringArea>(d => d.WateringAreas).WithOne(ds => ds.DeviceConfiguration).OnDelete(DeleteBehavior.Cascade);
        }
    }
}