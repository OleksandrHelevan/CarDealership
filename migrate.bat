@echo off
REM Entity Framework Migration Helper for CarDealership
REM This batch file helps manage database migrations

if "%1"=="" goto :help
if "%1"=="help" goto :help
if "%1"=="add-migration" goto :add_migration
if "%1"=="update-database" goto :update_database
if "%1"=="remove-migration" goto :remove_migration
if "%1"=="list-migrations" goto :list_migrations
if "%1"=="database-drop" goto :drop_database
if "%1"=="database-create" goto :create_database
goto :help

:add_migration
if "%2"=="" (
    echo Error: Migration name is required for add-migration
    echo Usage: migrate.bat add-migration MigrationName
    goto :end
)
echo Adding migration: %2
dotnet ef migrations add %2 --project CarDealership --startup-project CarDealership --context DealershipContext
goto :end

:update_database
echo Updating database...
dotnet ef database update --project CarDealership --startup-project CarDealership --context DealershipContext
goto :end

:remove_migration
echo Removing last migration...
dotnet ef migrations remove --project CarDealership --startup-project CarDealership --context DealershipContext
goto :end

:list_migrations
echo Listing migrations...
dotnet ef migrations list --project CarDealership --startup-project CarDealership --context DealershipContext
goto :end

:drop_database
echo Dropping database...
dotnet ef database drop --project CarDealership --startup-project CarDealership --context DealershipContext --force
goto :end

:create_database
echo Creating database...
dotnet ef database create --project CarDealership --startup-project CarDealership --context DealershipContext
goto :end

:help
echo Entity Framework Migration Helper
echo ================================
echo.
echo Usage: migrate.bat ^<action^> [migration-name]
echo.
echo Actions:
echo   add-migration    - Add a new migration
echo   update-database  - Apply pending migrations to database
echo   remove-migration - Remove the last migration
echo   list-migrations  - List all migrations
echo   database-drop    - Drop the database
echo   database-create  - Create the database
echo   help             - Show this help message
echo.
echo Examples:
echo   migrate.bat add-migration InitialCreate
echo   migrate.bat update-database
echo   migrate.bat list-migrations
echo.

:end
pause
