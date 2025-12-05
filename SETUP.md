# Setup Guide

Quick guide to set up and verify the Clean Architecture Boilerplate.

---

## Quick Setup Checklist

### Local Development

- [ ] .NET 8 SDK installed ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- [ ] Repository cloned
- [ ] `dotnet restore` completed
- [ ] `dotnet build` succeeds
- [ ] `dotnet test` passes (20+ tests)
- [ ] API runs: `cd src/Api && dotnet run`
- [ ] Swagger opens: `https://localhost:7286/swagger`

### GitHub Setup

- [ ] Repository created on GitHub
- [ ] Code pushed to `main`
- [ ] Branch protection configured (see below)
- [ ] GitHub Actions workflows running

---

## Verification Commands

Run these to verify everything works:
```bash
# Build
dotnet build --configuration Release

# Test
dotnet test --verbosity normal

# Format check
dotnet format --verify-no-changes

# Run API
cd src/Api
dotnet run
```

**Open browser:** `https://localhost:7286/swagger`

**Test endpoint:** POST `/api/todo` with:
```json
{
  "title": "Test Todo",
  "description": "Verify it works"
}
```

Expected: `201 Created` ✅

---

## Database Configuration

### Development (Default)

Uses **in-memory database**. No setup needed! 🎉

### Production (SQL Server)

When ready for production:

**1. Update `Infrastructure/DependencyInjection.cs`:**
```csharp
// Change from:
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("BoilerplateDb"));

// To:
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
```

**2. Add connection string to `appsettings.json`:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=YourDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

**3. Create and run migrations:**
```bash
# Install EF tools (one time)
dotnet tool install --global dotnet-ef

# Create migration
cd src/Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../Api

# Update database
dotnet ef database update --startup-project ../Api
```

---

## GitHub Branch Protection

### Protect `main` Branch

**Settings → Branches → Add rule**

**Branch name pattern:** `main`

**Enable:**
- ✅ Require pull request before merging (0 or 1 approval)
- ✅ Require status checks to pass:
  - `build-and-test`
  - `format-check`
  - `validate-pr`
- ✅ Require branches to be up to date
- ✅ Require conversation resolution before merging
- ❌ No force pushes
- ❌ No deletions

### Protect `develop` Branch (Optional)

**Branch name pattern:** `develop`

**Enable:**
- ✅ Require status checks to pass
- ❌ No force pushes
- ❌ No deletions

---

## Adapting for Your Project

Fork this for a new project:

### 1. Clone and Rename
```bash
git clone https://github.com/yourusername/your-project-api.git
cd your-project-api
```

### 2. Replace TodoItem with Your Entities

**Delete:**
- `src/Domain/Entities/TodoItem.cs`
- `src/Application/TodoItems/`
- `src/Application/DTOs/TodoItemDto.cs`
- `src/Api/Controllers/TodoController.cs`

**Create your entities:**
- `src/Domain/Entities/YourEntity.cs`
- `src/Application/YourEntity/Commands/...`
- `src/Application/YourEntity/Queries/...`
- `src/Api/Controllers/YourController.cs`

### 3. Update DbContext
```csharp
// Infrastructure/Data/ApplicationDbContext.cs
public DbSet<YourEntity> YourEntities { get; set; }
// Remove: public DbSet<TodoItem> TodoItems { get; set; }
```

### 4. Update Tests

Replace test files with tests for your new features.

### 5. Update Documentation

- [ ] `README.md` - Project name and description
- [ ] `SPECS.md` - New API endpoints
- [ ] `CHANGELOG.md` - Reset to v1.0.0

---

## Common Issues

### Tests fail with mock errors

**Fix:** Check mock setups use `.ReturnsAsync()` for async methods

### Swagger doesn't show XML comments

**Fix:** Ensure `Api.csproj` has:
```xml
<GenerateDocumentationFile>true</GenerateDocumentationFile>
```

### Can't push to main

**Fix:** Branch protection working! Create a PR instead:
```bash
git checkout -b feature/your-feature
git push origin feature/your-feature
```

### Format check fails in CI

**Fix:** Run locally and commit:
```bash
dotnet format
git add .
git commit -m "style: apply code formatting"
git push
```

### MediatR handlers not found

**Fix:** Check `Application/DependencyInjection.cs` registers correct assembly:
```csharp
cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
```

---

**Need more help?** Check:
- [ARCHITECTURE.md](ARCHITECTURE.md) - Architecture details
- [SPECS.md](SPECS.md) - API documentation
- [CONTRIBUTING.md](CONTRIBUTING.md) - Contribution guidelines

**Happy coding! 🚀**
