# Bollelisten

A .NET 9 distributed application built with .NET Aspire for tracking incidents and managing groups and users.

## 🏗️ Architecture

This project uses .NET Aspire for orchestration and follows a clean architecture pattern with separate projects for different concerns:

### Projects

- **AppHost** - .NET Aspire orchestration host that manages service dependencies and infrastructure
- **API** - ASP.NET Core Web API providing REST endpoints
- **MigrationService** - Background service for database migrations and initialization
- **Data** - Entity Framework Core data models and DbContext
- **Common** - Shared utilities and extension methods
- **ServiceDefaults** - Common service configurations and defaults

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for PostgreSQL container)
- IDE: Visual Studio 2022, JetBrains Rider, or VS Code

### Running the Application

1. **Set user secrets for database credentials**
   
   Navigate to the AppHost project and set the database password. The `admin-username` is already set in `appsettings.Development.json`:
   ```bash
   cd AppHost
   dotnet user-secrets set "Parameters:admin-password" "your-password"
   ```

3. **Run the AppHost project**
   ```bash
   dotnet run --project AppHost
   ```
   
   This will:
   - Start a PostgreSQL container on port 55483
   - Start PgWeb (PostgreSQL admin tool) on port 55484
   - Run database migrations via MigrationService
   - Start the API service

4. **Access the application**
   - API: Check the Aspire dashboard for the API endpoint
   - Aspire Dashboard: Default at `http://localhost:15888` (or shown in console)
   - PgWeb: `http://localhost:55484`

### Migrations

Migrations are automatically run on startup by the MigrationService. To add new migrations:

```bash
cd Data
dotnet ef migrations add <MigrationName> --startup-project ../MigrationService
```

## 🔧 Configuration

### AppHost Configuration

The AppHost configures the following infrastructure:

```csharp
- PostgreSQL: Port 55483
- PgWeb: Port 55484
- Connection string name: "bollelisten"
```

### Connection Strings

Connection strings are automatically injected by .NET Aspire. The database connection name is `"bollelisten"`.

## 🛠️ Development


### Adding New Features

1. **Add new entities** in `Data/Entities/`
2. **Update DbContext** in `Data/AppDbContext.cs`
3. **Create migration** using EF Core CLI
4. **Add controllers** in `API/Controllers/`
5. **Update services** as needed

### Building the Solution

```bash
dotnet build Bollelisten.sln
```

### Running Tests

```bash
dotnet test Bollelisten.sln
```
