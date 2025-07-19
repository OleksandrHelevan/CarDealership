using CarDealership.entity;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.config
{
    public class DealershipContext : DbContext
    {
        public DbSet<GasolineEngine> GasolineEngines { get; set; }
        public DbSet<ElectroEngine> ElectroEngines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=car_dealership;Username=postgres;Password=1234qwer");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GasolineEngine>()
                .ToTable("gasoline_engines");  // Вказуємо точну назву таблиці

            modelBuilder.Entity<ElectroEngine>()
                .ToTable("electro_engines");   // Вказуємо точну назву таблиці

            // Якщо є енум, який зберігається як текст, можна додати конвертацію
            modelBuilder.Entity<GasolineEngine>()
                .Property(e => e.FuelType)
                .HasConversion<string>();

            modelBuilder.Entity<ElectroEngine>()
                .Property(e => e.MotorType)
                .HasConversion<string>();
        }

    }
}