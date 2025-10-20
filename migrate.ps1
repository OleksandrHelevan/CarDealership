# Entity Framework Migration Helper Script
# This script helps manage database migrations for the CarDealership project

param(
    [Parameter(Mandatory=$false)]
    [string]$Action = "help",
    
    [Parameter(Mandatory=$false)]
    [string]$MigrationName = "",
    
    [Parameter(Mandatory=$false)]
    [string]$ConnectionString = "Host=localhost;Port=5432;Database=car_dealership;Username=postgres;Password=1234qwer"
)

$ProjectPath = "CarDealership"
$StartupProject = "CarDealership"

function Show-Help {
    Write-Host "Entity Framework Migration Helper" -ForegroundColor Green
    Write-Host "=================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Usage: .\migrate.ps1 -Action <action> [-MigrationName <name>]" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Actions:" -ForegroundColor Cyan
    Write-Host "  add-migration    - Add a new migration" -ForegroundColor White
    Write-Host "  update-database  - Apply pending migrations to database" -ForegroundColor White
    Write-Host "  remove-migration - Remove the last migration" -ForegroundColor White
    Write-Host "  list-migrations  - List all migrations" -ForegroundColor White
    Write-Host "  database-drop    - Drop the database" -ForegroundColor White
    Write-Host "  database-create  - Create the database" -ForegroundColor White
    Write-Host "  help             - Show this help message" -ForegroundColor White
    Write-Host ""
    Write-Host "Examples:" -ForegroundColor Cyan
    Write-Host "  .\migrate.ps1 -Action add-migration -MigrationName InitialCreate" -ForegroundColor White
    Write-Host "  .\migrate.ps1 -Action update-database" -ForegroundColor White
    Write-Host "  .\migrate.ps1 -Action list-migrations" -ForegroundColor White
}

function Add-Migration {
    param([string]$Name)
    
    if ([string]::IsNullOrEmpty($Name)) {
        Write-Host "Error: Migration name is required for add-migration action" -ForegroundColor Red
        return
    }
    
    Write-Host "Adding migration: $Name" -ForegroundColor Green
    dotnet ef migrations add $Name --project $ProjectPath --startup-project $StartupProject --context DealershipContext
}

function Update-Database {
    Write-Host "Updating database..." -ForegroundColor Green
    dotnet ef database update --project $ProjectPath --startup-project $StartupProject --context DealershipContext
}

function Remove-Migration {
    Write-Host "Removing last migration..." -ForegroundColor Yellow
    dotnet ef migrations remove --project $ProjectPath --startup-project $StartupProject --context DealershipContext
}

function List-Migrations {
    Write-Host "Listing migrations..." -ForegroundColor Green
    dotnet ef migrations list --project $ProjectPath --startup-project $StartupProject --context DealershipContext
}

function Drop-Database {
    Write-Host "Dropping database..." -ForegroundColor Red
    dotnet ef database drop --project $ProjectPath --startup-project $StartupProject --context DealershipContext --force
}

function Create-Database {
    Write-Host "Creating database..." -ForegroundColor Green
    dotnet ef database create --project $ProjectPath --startup-project $StartupProject --context DealershipContext
}

# Main execution
switch ($Action.ToLower()) {
    "add-migration" {
        Add-Migration -Name $MigrationName
    }
    "update-database" {
        Update-Database
    }
    "remove-migration" {
        Remove-Migration
    }
    "list-migrations" {
        List-Migrations
    }
    "database-drop" {
        Drop-Database
    }
    "database-create" {
        Create-Database
    }
    "help" {
        Show-Help
    }
    default {
        Write-Host "Unknown action: $Action" -ForegroundColor Red
        Show-Help
    }
}
