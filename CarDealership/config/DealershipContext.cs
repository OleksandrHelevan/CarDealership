using CarDealership.entity;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.config;
public class DealershipContext : DbContext
{
    public DbSet<GasolineEngineEntity> GasolineEnginesEntity { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=car_dealership;Username=postgres;Password=1234qwer");
    }
}
