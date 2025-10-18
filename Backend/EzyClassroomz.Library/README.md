# EzyClassroomz.Library - Database Setup

## Overview
This library contains the Entity Framework Core data layer for EzyClassroomz, configured to use PostgreSQL with .NET 9.0.

## Technology Stack
- **Framework**: .NET 9.0
- **ORM**: Entity Framework Core 9.0.10
- **Database**: PostgreSQL (via Npgsql.EntityFrameworkCore.PostgreSQL 9.0.4)
- **Configuration**: Microsoft.Extensions.Configuration 9.0.10

## Project Structure

```
EzyClassroomz.Library/
├── Data/
│   ├── ApplicationDbContext.cs           # Main DbContext
│   └── ApplicationDbContextFactory.cs     # Design-time factory for migrations
├── Entities/
│   └── User.cs                           # User entity
├── Migrations/
│   ├── 20251016123928_202510161339_users.cs
│   └── ApplicationDbContextModelSnapshot.cs
├── Repositories/                          # (Reserved for future use)
├── appsettings.json                      # Database connection configuration
└── EzyClassroomz.Library.csproj
```

## Database Configuration

### Connection String
The connection string is stored in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ezyclassroomz;Username=root;Password=password"
  }
}
```

### DbContext Factory
The `ApplicationDbContextFactory` is configured to:
- Load configuration from `appsettings.json`
- Support environment variables
- Enable design-time tooling for migrations

## Migrations

### Current Migrations
1. **20251016123928_202510161339_users** (Initial Migration)
   - Creates `Users` table
   - Includes composite unique index on (Email, Name)
   - Includes unique index on Name

### Applying Migrations to Database
To apply migrations to your PostgreSQL database:

```powershell
cd EzyClassroomz.Library
dotnet ef database update
```

### Listing Migrations
To view all migrations:

```powershell
dotnet ef migrations list
```

### Creating New Migrations
When you add or modify entities, create a new migration:

```powershell
dotnet ef migrations add YourMigrationName
```

### Removing the Last Migration
If you need to remove the last unapplied migration:

```powershell
dotnet ef migrations remove
```

## Database Structure

### Current Entities

#### User
- **Id** (bigint, primary key, auto-increment)
  - Generated using PostgreSQL identity column strategy
- **Name** (varchar(256), required)
  - Unique constraint
  - Part of composite unique index with Email
- **Email** (varchar(256), required)
  - Part of composite unique index with Name
- **PasswordHash** (varchar(256), required)
  - Stores hashed password for authentication

### Indexes
- **PK_Users**: Primary key on Id
- **IX_Users_Email_Name**: Composite unique index on (Email, Name)
- **IX_Users_Name**: Unique index on Name

## Prerequisites

1. **PostgreSQL Server**: Version compatible with Npgsql 9.0.4
2. **Database**: `ezyclassroomz` database (created automatically on first migration)
3. **.NET 9.0 SDK**: Required for building and running
4. **EF Core Tools**: Install globally if not already installed:
   ```powershell
   dotnet tool install --global dotnet-ef
   ```

## Usage in Applications

### Adding Project Reference
To use this library in your application (e.g., EzyClassroomz.Api):

```powershell
dotnet add reference ../EzyClassroomz.Library/EzyClassroomz.Library.csproj
```

### Registering DbContext
In your `Program.cs` or startup configuration:

```csharp
using EzyClassroomz.Library.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### Configuration in API Project
Add the connection string to your API's `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ezyclassroomz;Username=root;Password=password"
  }
}
```

### Using DbContext in Controllers/Services
```csharp
public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByIdAsync(long id)
    {
        return await _context.Users.FindAsync(id);
    }
}
```

## Development Notes

- **Nullable Reference Types**: Enabled in project
- **Implicit Usings**: Enabled for cleaner code
- **Reserved Directory**: `Repositories/` folder is reserved for future repository pattern implementation

## Troubleshooting

### Migration Issues
If migrations fail to apply:
1. Verify PostgreSQL server is running
2. Check connection string in `appsettings.json`
3. Ensure database user has sufficient permissions
4. Review migration files for conflicts

### Connection Issues
If you encounter connection errors:
1. Verify PostgreSQL is listening on the correct port (default: 5432)
2. Check firewall settings
3. Validate credentials in connection string
4. Test connection using psql or pgAdmin
