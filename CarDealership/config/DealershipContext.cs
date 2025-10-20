using CarDealership.entity;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.config
{
    public class DealershipContext : DbContext
    {
        public DealershipContext() { }

        public DealershipContext(DbContextOptions<DealershipContext> options) : base(options) { }

        // === DbSets ===
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<PassportData> PassportData { get; set; }
        public DbSet<AuthorizationRequest> AuthorizationRequests { get; set; }

        // Car hierarchy (TPH)
        public DbSet<Car> Cars { get; set; }

        // Engines
        public DbSet<GasolineEngine> GasolineEngines { get; set; }
        public DbSet<ElectroEngine> ElectroEngines { get; set; }

        // Products & Orders
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        // === CONFIG ===
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(
                    "Host=localhost;Port=5432;Database=car_dealership;Username=postgres;Password=1234qwer");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === TABLES ===
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Client>().ToTable("clients");
            modelBuilder.Entity<PassportData>().ToTable("passport_data");
            modelBuilder.Entity<AuthorizationRequest>().ToTable("authorization_requests");
            modelBuilder.Entity<Car>().ToTable("cars");
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<GasolineEngine>().ToTable("gasoline_engines");
            modelBuilder.Entity<ElectroEngine>().ToTable("electro_engines");

            // === INHERITANCE (TPH) ===
            modelBuilder.Entity<Car>()
                .HasDiscriminator<CarType>("car_type")
                .HasValue<GasolineCar>(CarType.Gasoline)
                .HasValue<ElectroCar>(CarType.Electro);

            // === RELATIONSHIPS ===
            ConfigureUserRelationships(modelBuilder);
            ConfigureCarRelationships(modelBuilder);
            ConfigureProductRelationships(modelBuilder);
            ConfigureOrderRelationships(modelBuilder);

            // === ENUMS / CONSTRAINTS / INDEXES ===
            ConfigureEnumConversions(modelBuilder);
            ConfigureConstraints(modelBuilder);
            ConfigureIndexes(modelBuilder);
        }

        // === RELATIONSHIPS ===

        private void ConfigureUserRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)
                .WithOne(u => u.Client)
                .HasForeignKey<Client>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.PassportData)
                .WithMany(p => p.Clients)
                .HasForeignKey(c => c.PassportDataId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureCarRelationships(ModelBuilder modelBuilder)
        {
            // GasolineCar -> GasolineEngine
            modelBuilder.Entity<GasolineCar>()
                .HasOne(gc => gc.Engine)
                .WithMany(ge => ge.GasolineCars)
                .HasForeignKey(gc => gc.EngineId)
                .OnDelete(DeleteBehavior.Restrict);

            // ElectroCar -> ElectroEngine
            modelBuilder.Entity<ElectroCar>()
                .HasOne(ec => ec.Engine)
                .WithMany(ee => ee.ElectroCars)
                .HasForeignKey(ec => ec.EngineId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureProductRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Car)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CarId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureOrderRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        // === INDEXES ===

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<PassportData>().HasIndex(p => p.PassportNumber).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.ProductNumber).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => new { p.CarId, p.CarType });
            modelBuilder.Entity<Order>().HasIndex(o => o.OrderDate);
            modelBuilder.Entity<Order>().HasIndex(o => o.Status);
        }

        // === ENUM CONVERSIONS ===

        private void ConfigureEnumConversions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.AccessRight)
                .HasConversion<string>();

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

            modelBuilder.Entity<GasolineEngine>()
                .Property(e => e.FuelType)
                .HasConversion<string>();

            modelBuilder.Entity<ElectroEngine>()
                .Property(e => e.MotorType)
                .HasConversion<string>();

            modelBuilder.Entity<Product>()
                .Property(p => p.CarType)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentType)
                .HasConversion<string>();
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();
        }

        // === CONSTRAINTS ===

        private void ConfigureConstraints(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .Property(c => c.Price)
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            modelBuilder.Entity<Car>()
                .HasCheckConstraint("CK_Cars_Price_Positive", "price >= 0");

            modelBuilder.Entity<GasolineEngine>()
                .Property(e => e.Power)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ElectroEngine>()
                .Property(e => e.Power)
                .HasPrecision(8, 2);
        }

        // === SaveChanges Overrides ===

        public override int SaveChanges()
        {
            NormalizeDateTimeKinds();
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            NormalizeDateTimeKinds();
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void NormalizeDateTimeKinds()
        {
            foreach (var entry in ChangeTracker.Entries()
                         .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                foreach (var prop in entry.Properties)
                {
                    if (prop.CurrentValue is DateTime dt)
                        prop.CurrentValue = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                }
            }
        }

        private void UpdateAuditFields()
        {
            foreach (var entry in ChangeTracker.Entries()
                         .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                var now = DateTime.UtcNow;
                switch (entry.Entity)
                {
                    case Car car:
                        if (entry.State == EntityState.Added) car.CreatedAt = now;
                        else car.UpdatedAt = now;
                        break;
                    case Product p:
                        if (entry.State == EntityState.Added) p.CreatedAt = now;
                        else p.UpdatedAt = now;
                        break;
                    case Order o:
                        if (entry.State == EntityState.Added) o.CreatedAt = now;
                        else o.UpdatedAt = now;
                        break;
                }
            }
        }
    }
}
