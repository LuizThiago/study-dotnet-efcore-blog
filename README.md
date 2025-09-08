## Study: .NET + EF Core (Fluent Mapping) — Blog Sample

A small learning project showcasing Entity Framework Core Fluent Mapping with a simple blog domain.

### Notes
- This repository is for learning purposes. The connection string is hard-coded for convenience—consider environment variables or user secrets in real projects.
- The sample uses simple console output for demonstration.

### What this project demonstrates
- **EF Core Fluent Mapping** with configuration classes per entity (`Data/Mappings/*Map.cs`).
- A **clean domain model** (`Models/`) with relationships:
  - One-to-many: `User (Author)` → `Post`, `Category` → `Post`.
  - Many-to-many: `Post` ↔ `Tag`, `User` ↔ `Role`.
- **Migrations** already created (see `Migrations/`), ready to apply to SQL Server.
- A minimal console app (`Program.cs`) with helper methods to create and query data.

### Tech stack
- .NET SDK: **9.0**
- EF Core: **9.0.8** (`Microsoft.EntityFrameworkCore`, `Design`, `SqlServer`)
- Database: **SQL Server** (local instance or Docker container)

### Repository layout
```
Data/
  BlogDataContext.cs            # DbContext + connection string
  Mappings/                     # Fluent mappings per entity
    CategoryMap.cs
    PostMap.cs
    RoleMap.cs
    TagMap.cs
    UserMap.cs
Migrations/                     # Initial migration (schema + model snapshot)
Models/                         # Domain entities (POCOs)
  Category.cs
  Post.cs
  Role.cs
  Tag.cs
  User.cs
Program.cs                      # Minimal console app with CRUD helpers
Blog.csproj                     # TargetFramework net9.0 + EF packages
```

### Prerequisites
- .NET SDK 9.0
- SQL Server available at `localhost,1433` (default in this repo).
- Optional: Docker Desktop to run SQL Server in a container.

### Configure the database (recommended via Docker)
If you do not have a local SQL Server, start one with Docker:

```bash
docker pull mcr.microsoft.com/mssql/server:2022-latest
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=1q2w3e4r@#$" -p 1433:1433 \
  --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

The project uses the following connection string in `Data/BlogDataContext.cs`:
```
Server=localhost,1433;Database=FluentBlog;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True;
```
Update it if your environment is different.

### Restore, create database, and run
1) Restore packages
```bash
dotnet restore
```

2) (First run) Install EF Core CLI if needed
```bash
dotnet tool install --global dotnet-ef
```

3) Apply migrations to create the database
```bash
dotnet ef database update
```

4) Run the app
```bash
dotnet run
```

By default, `Program.cs` fetches posts and prints them. You can use the provided helpers to create data.

### Domain model at a glance
- `User` (Author) has many `Post`s and many `Role`s (many-to-many via `UserRole`).
- `Category` has many `Post`s.
- `Post` belongs to one `User` (Author) and one `Category` and has many `Tag`s (many-to-many via `PostTag`).
- Unique indexes on `Slug` for `Category`, `Post`, and `User`.

### Fluent mappings
Mappings live under `Data/Mappings/` and configure:
- Table names, keys, columns (type/length), required flags, defaults.
- Indexes and relationship constraints.

Example highlights:
- `PostMap` sets defaults for `CreateDate` and `LastUpdateDate` via `GETDATE()`.
- Many-to-many using `UsingEntity<Dictionary<string, object>>` for join tables (`PostTag`, `UserRole`).

### Common EF Core commands
```bash
# Add a new migration
dotnet ef migrations add <MigrationName>

# Apply migrations to the database
dotnet ef database update

# Remove last migration (if not applied)
dotnet ef migrations remove
```