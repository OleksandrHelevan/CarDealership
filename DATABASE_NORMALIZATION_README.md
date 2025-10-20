# Car Dealership Database Schema Normalization & Migration Setup

## Overview

This project has been refactored to follow proper database normalization principles and includes Entity Framework migrations for automatic database schema management.

## Key Improvements Made

### 1. Database Normalization
- **Eliminated Code Duplication**: Created a base `Car` entity that both `GasolineCar` and `ElectroCar` inherit from
- **Proper Inheritance**: Used Entity Framework's Table-Per-Hierarchy (TPH) inheritance pattern
- **Improved Relationships**: Fixed foreign key constraints and navigation properties
- **Added Audit Fields**: All entities now have `CreatedAt` and `UpdatedAt` timestamps
- **Better Constraints**: Added check constraints for data validation

### 2. Entity Structure Changes

#### Base Car Entity
```csharp
public abstract class Car
{
    // Common properties: Brand, ModelName, Color, Mileage, Price, etc.
    // Audit fields: CreatedAt, UpdatedAt
    // Discriminator: CarType (Gasoline/Electric)
}
```

#### Improved Entities
- **User**: Added password hashing, audit fields, and better constraints
- **Product**: Now uses polymorphic relationship with Car instead of nullable foreign keys
- **Order**: Enhanced with status tracking, delivery information, and audit fields
- **PassportData**: Extended with additional contact information
- **AuthorizationRequest**: Added processing tracking and audit fields

### 3. Database Configuration
- **Proper Indexes**: Added performance indexes on frequently queried fields
- **Enum Conversions**: All enums are stored as strings for better readability
- **Check Constraints**: Added validation constraints for data integrity
- **Audit Trail**: Automatic timestamp management for all entities

## Migration Setup

### Prerequisites
1. Install .NET 9.0 SDK
2. Install PostgreSQL
3. Ensure Entity Framework Tools are available

### Database Connection
The application uses PostgreSQL with the following connection string:
```
Host=localhost;Port=5432;Database=car_dealership;Username=postgres;Password=1234qwer
```

### Migration Commands

#### Using the PowerShell Script
```powershell
# Add a new migration
.\migrate.ps1 -Action add-migration -MigrationName "InitialCreate"

# Apply migrations to database
.\migrate.ps1 -Action update-database

# List all migrations
.\migrate.ps1 -Action list-migrations

# Drop database (careful!)
.\migrate.ps1 -Action database-drop
```

#### Using dotnet ef commands directly
```bash
# Navigate to project directory
cd CarDealership

# Add initial migration
dotnet ef migrations add InitialCreate --context DealershipContext

# Apply migrations
dotnet ef database update --context DealershipContext

# List migrations
dotnet ef migrations list --context DealershipContext
```

## Automatic Database Initialization

The application now includes automatic database initialization:

1. **Database Creation**: Ensures the database exists
2. **Migration Application**: Applies any pending migrations
3. **Seed Data**: Creates initial admin user and sample data

### Initial Data
- **Admin User**: 
  - Login: `admin`
  - Password: `admin123`
  - Role: Admin
- **Sample Cars**: Toyota Camry (Gasoline) and Tesla Model 3 (Electric)
- **Sample Products**: Products linked to the sample cars

## Database Schema

### Tables Created
- `users` - User accounts and authentication
- `clients` - Client information linked to users
- `passport_data` - Personal information for clients
- `authorization_requests` - User registration requests
- `cars` - Base car information (with discriminator)
- `gasoline_cars` - Gasoline-specific car data
- `electro_cars` - Electric-specific car data
- `gasoline_engines` - Gasoline engine specifications
- `electro_engines` - Electric engine specifications
- `products` - Car products for sale
- `orders` - Customer orders

### Key Relationships
- User ↔ Client (1:1)
- Client ↔ PassportData (1:1)
- Car → Engine (1:1, polymorphic)
- Product → Car (1:1, polymorphic)
- Order → Client (1:1)
- Order → Product (1:1)

## Running the Application

1. **Start PostgreSQL** service
2. **Run the application** - Database will be automatically initialized
3. **Login** with admin credentials or create new accounts

## Migration Management

### Creating New Migrations
When you make changes to entities:
1. Add/modify entity properties
2. Run: `dotnet ef migrations add <MigrationName>`
3. Review the generated migration
4. Apply: `dotnet ef database update`

### Best Practices
- Always review generated migrations before applying
- Test migrations on development database first
- Keep migrations small and focused
- Use descriptive migration names
- Backup database before major migrations

## Troubleshooting

### Common Issues
1. **Connection String**: Ensure PostgreSQL is running and credentials are correct
2. **Migration Conflicts**: Resolve conflicts by removing conflicting migrations
3. **Entity Changes**: Ensure all entity changes are properly configured in DbContext

### Reset Database
If you need to start fresh:
```bash
dotnet ef database drop --force
dotnet ef database update
```

## Performance Optimizations

### Indexes Added
- User login (unique)
- Passport number (unique)
- Product number (unique)
- Order date and status
- Authorization request status and date

### Constraints Added
- Price > 0
- Power > 0
- Fuel consumption > 0
- Battery capacity > 0
- Year between 1900-2030
- Mileage >= 0
- Weight > 0
- Number of doors > 0

This normalized schema provides better data integrity, performance, and maintainability for your car dealership application.
