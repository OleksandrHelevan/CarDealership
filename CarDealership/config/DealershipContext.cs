using CarDealership.entity;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.config
{
    public class DealershipContext : DbContext
    {
        public DbSet<GasolineEngine> GasolineEngines { get; set; }
        public DbSet<ElectroEngine> ElectroEngines { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ElectroCar> ElectroCars { get; set; }
        public DbSet<GasolineCar> GasolineCars { get; set; }
        public DbSet<AuthorizationRequest> AuthorizationRequests { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<PassportData> PassportData { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=car_dealership;Username=postgres;Password=1234qwer");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GasolineEngine>()
                .ToTable("gasoline_engines"); // Вказуємо точну назву таблиці

            modelBuilder.Entity<ElectroEngine>()
                .ToTable("electro_engines"); // Вказуємо точну назву таблиці

            modelBuilder.Entity<ElectroEngine>()
                .Property(e => e.MotorType)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .ToTable("keys") // Назва таблиці в БД
                .Property(u => u.AccessRight)
                .HasConversion<string>(); // enum AccessRight => string

            modelBuilder.Entity<ElectroCar>()
                .HasOne(e => e.Engine)
                .WithMany()
                .HasForeignKey("engine_id")
                .IsRequired();

            modelBuilder.Entity<GasolineCar>()
                .HasOne(e => e.Engine)
                .WithMany()
                .HasForeignKey("engine_id")
                .IsRequired();

            // Якщо є енум, який зберігається як текст, можна додати конвертацію
            modelBuilder.Entity<GasolineEngine>()
                .Property(e => e.FuelType)
                .HasConversion<string>();

            modelBuilder.Entity<AuthorizationRequest>()
                .ToTable("requests"); // Назва таблиці в БД

            // Якщо Request.Status — це enum, додай і для нього:
            modelBuilder.Entity<AuthorizationRequest>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Product>()
                .Property(p => p.CarType)
                .HasConversion<string>();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ElectroCar)
                .WithMany()
                .HasForeignKey("electro_car_id");

            modelBuilder.Entity<Product>()
                .HasOne(p => p.GasolineCar)
                .WithMany()
                .HasForeignKey("gasoline_car_id");
        }
    }
}