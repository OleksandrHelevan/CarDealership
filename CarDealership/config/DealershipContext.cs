using CarDealership.entity;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.config;
public class DealershipContext : DbContext
{
    public DbSet<GasolineEngineEntity> GasolineEngines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=car_dealership;Username=postgres;Password=1234qwer");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GasolineEngineEntity>()
            .Property(e => e.FuelType)
            .HasConversion<string>();
    }

}
