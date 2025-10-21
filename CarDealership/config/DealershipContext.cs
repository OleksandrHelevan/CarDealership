using CarDealership.entity;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarDealership.config
{
    public class DealershipContext : DbContext
    {
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PassportData> PassportData { get; set; }
        public DbSet<AuthorizationRequest> AuthorizationRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=car_dealership_norm;Username=postgres;Password=1234qwer");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var carTypeConverter = new ValueConverter<CarType, string>(
                v => v.ToString().ToLower(),
                v => (CarType)Enum.Parse(typeof(CarType), v, true));

            var engineTypeConverter = new ValueConverter<EngineType, string>(
                v => v.ToString().ToLower(),
                v => (EngineType)Enum.Parse(typeof(EngineType), v, true));

            var utcConverter = new ValueConverter<DateTime, DateTime>(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableUtcConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);
            modelBuilder.Entity<AuthorizationRequest>()
                .ToTable("requests");

            modelBuilder.Entity<Order>()
                .ToTable("orders");

            //FK
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Engine)
                .WithMany()
                .HasForeignKey(c => c.EngineId)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Car)
                .WithMany()
                .HasForeignKey(p => p.CarId)
                .IsRequired();
            

            modelBuilder.Entity<Order>()
                .ToTable("orders")
                .HasOne(o => o.Client)      // Навігація
                .WithMany()                 // Clients не має колекції Orders, тому просто WithMany()
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)     // Навігація
                .WithMany()                 // Products не має колекції Orders
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderDate)
                .HasConversion(utcConverter);

            modelBuilder.Entity<Product>()
                .Property(p => p.AvailableFrom)
                .HasConversion(nullableUtcConverter);

            //Enums
            modelBuilder.Entity<Car>()
                .Property(c => c.Color)
                .HasConversion<string>();

            modelBuilder.Entity<Car>()
                .Property(c => c.DriveType)
                .HasConversion<string>();

            modelBuilder.Entity<Car>()
                .Property(c => c.Transmission)
                .HasConversion<string>();

            modelBuilder.Entity<Car>()
                .Property(c => c.BodyType)
                .HasConversion<string>();

            modelBuilder.Entity<Car>()
                .Property(c => c.CarType)
                .HasConversion(carTypeConverter);

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentType)
                .HasConversion<string>();
            
            modelBuilder.Entity<AuthorizationRequest>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Engine>()
                .Property(e => e.FuelType)
                .HasConversion<string>();

            modelBuilder.Entity<Engine>()
                .Property(e => e.MotorType)
                .HasConversion<string>();

            modelBuilder.Entity<Engine>()
                .Property(e => e.EngineType)
                .HasConversion(engineTypeConverter);

            modelBuilder.Entity<User>()
                .ToTable("keys")
                .Property(u => u.AccessRight)
                .HasConversion<string>();
        }
    }
}
