using CarDealership.entity;
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
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PassportData> PassportData { get; set; }
        public DbSet<AuthorizationRequest> AuthorizationRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=car_dealership;Username=postgres;Password=1234qwer");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            //Entity
            
            modelBuilder.Entity<GasolineEngine>()
                .ToTable("gasoline_engines");

            modelBuilder.Entity<ElectroEngine>()
                .ToTable("electro_engines");
            
            modelBuilder.Entity<AuthorizationRequest>()
                .ToTable("requests");
                
            modelBuilder.Entity<Order>()
                .ToTable("orders");

            //FK
            
            modelBuilder.Entity<ElectroCar>()
                .HasOne(c => c.Engine)
                .WithMany()
                .HasForeignKey(c => c.EngineId)
                .IsRequired();

            modelBuilder.Entity<GasolineCar>()
                .HasOne(c => c.Engine)
                .WithMany()
                .HasForeignKey(c => c.EngineId)
                .IsRequired();

            
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ElectroCar)
                .WithMany()
                .HasForeignKey("electro_car_id");

            modelBuilder.Entity<Product>()
                .HasOne(p => p.GasolineCar)
                .WithMany()
                .HasForeignKey("gasoline_car_id");
            

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany()
                .HasForeignKey("client_id")
                .IsRequired();

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey("product_id")
                .IsRequired();
            
            //Enums

            
            modelBuilder.Entity<ElectroCar>()
                .Property(c => c.Color)
                .HasConversion<string>();

            modelBuilder.Entity<ElectroCar>()
                .Property(c => c.DriveType)
                .HasConversion<string>();

            modelBuilder.Entity<ElectroCar>()
                .Property(c => c.Transmission)
                .HasConversion<string>();

            modelBuilder.Entity<ElectroCar>()
                .Property(c => c.BodyType)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentType)
                .HasConversion<string>();
            
            modelBuilder.Entity<GasolineCar>()
                .Property(c => c.Color)
                .HasConversion<string>();

            modelBuilder.Entity<GasolineCar>()
                .Property(c => c.DriveType)
                .HasConversion<string>();

            modelBuilder.Entity<GasolineCar>()
                .Property(c => c.Transmission)
                .HasConversion<string>();

            modelBuilder.Entity<GasolineCar>()
                .Property(c => c.BodyType)
                .HasConversion<string>();
            
            modelBuilder.Entity<AuthorizationRequest>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Product>()
                .Property(p => p.CarType)
                .HasConversion<string>();

            modelBuilder.Entity<GasolineEngine>()
                .Property(e => e.FuelType)
                .HasConversion<string>();

            modelBuilder.Entity<ElectroEngine>()
                .Property(e => e.MotorType)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .ToTable("keys")
                .Property(u => u.AccessRight)
                .HasConversion<string>();
        }
    }
}