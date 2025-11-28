# 🍽️ Savory - Personal Recipe Manager

A fullstack recipe management application where users can store, organize, and manage their personal recipes with ease.

[![.NET CI](https://github.com/GabbyFerm/Savory-Backend/actions/workflows/dotnet-ci.yml/badge.svg)](https://github.com/GabbyFerm/Savory-Backend/actions/workflows/dotnet-ci.yml)
[![Code Format](https://github.com/GabbyFerm/Savory-Backend/actions/workflows/code-format.yml/badge.svg)](https://github.com/GabbyFerm/Savory-Backend/actions/workflows/code-format.yml)
[![Discord](https://img.shields.io/badge/Discord-Notifications-7289DA?logo=discord&logoColor=white)](https://discord.com)

## 📋 Table of Contents

- [Overview](#-overview)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Features](#-features)
- [Database Schema](#-database-schema)
- [API Endpoints](#-api-endpoints)
- [Getting Started](#-getting-started)
- [Testing](#-testing)
- [Known Issues](#-known-issues)
- [Future Improvements](#-future-improvements)
- [Reflection](#-reflection)

## 🎯 Overview

Savory is a personal recipe manager built with .NET 8 and Clean Architecture principles. Users can create accounts, add their favorite recipes with ingredients, organize them by categories, upload images, and manage their personal cookbook digitally.

**Project Purpose:** School assignment for Object-Oriented Programming - Advanced

## 🛠️ Tech Stack

**Backend:**

- .NET 8 Web API
- Entity Framework Core 8
- SQL Server Express
- ASP.NET Core Identity (JWT Authentication)
- MediatR (CQRS Pattern)
- FluentValidation
- AutoMapper
- Serilog (Logging)
- xUnit + Moq + FluentAssertions (Testing)

**DevOps:**

- GitHub Actions (CI/CD)
- Swagger/OpenAPI Documentation

## 🏗️ Architecture

This project follows **Clean Architecture** with **CQRS** pattern:

```
┌─────────────────────────────────────────────┐
│          Api (Controllers, Middleware)      │
├─────────────────────────────────────────────┤
│   Application (Commands, Queries, DTOs)     │
├─────────────────────────────────────────────┤
│    Domain (Entities, Interfaces)            │
├─────────────────────────────────────────────┤
│  Infrastructure (EF Core, Repositories)     │
└─────────────────────────────────────────────┘
```

**Key Patterns:**

- **CQRS** with MediatR (Commands for writes, Queries for reads)
- **Repository Pattern** (Data access abstraction)
- **Service Layer** (Business logic separation)
- **Dependency Injection** (Per-layer configuration)
- **DTO Pattern** (API response/request objects)

## ✨ Features

### Core Features

- ✅ User registration and authentication with JWT tokens
- ✅ User profile management (username, email, password, avatar color)
- ✅ Recipe CRUD operations (Create, Read, Update, Delete)
- ✅ Ingredient management (Create, List, Search)
- ✅ Category listing (Read-only, seeded data)
- ✅ Recipe image upload (jpg, png, webp - max 5MB)
- ✅ Global error handling with structured responses
- ✅ Input validation with FluentValidation
- ✅ Authorization (users can only access their own recipes)

### Advanced Features

- ✅ Server-side filtering (by category, search term, ingredient name)
- ✅ Server-side sorting (by title, date, cook time - asc/desc)
- ✅ Dashboard statistics (total recipes, by category, averages, recent recipes)
- ✅ Custom exceptions with OperationResult pattern
- ✅ Comprehensive logging (console + file with Serilog)

## 🗄️ Database Schema

### Core Models

**User** (via Identity)

- Id (Guid)
- Email (string)
- Username (string)
- PasswordHash (string)
- AvatarColor (string) - Hex color for avatar background
- Recipes (Collection)

### Testing

- ✅ **11 unit tests**
  - Handler tests (Create, Update, Delete, GetById)
  - Mapping tests (AutoMapper configuration)
- ✅ **3 integration tests**
  - Auth flow (Register → Login → Access protected endpoint)
  - Recipe CRUD (Create and retrieve recipe)
  - Authorization (User cannot access other user's recipes)
- ✅ All tests pass in CI/CD pipeline

## 🗄️ Database Schema

### Core Models

**ApplicationUser** (ASP.NET Identity)

- Id (Guid, PK)
- UserName (string)
- Email (string)
- PasswordHash (string)
- AvatarColor (string) - Hex color for avatar
- Recipes (Collection)

**Recipe**

- Id (Guid, PK)
- UserId (Guid, FK → ApplicationUser)
- Title (string, max 200)
- Description (string, max 1000)
- Instructions (string)
- PrepTime (int, minutes)
- CookTime (int, minutes)
- Servings (int)
- ImagePath (string, nullable)
- CategoryId (Guid, FK → Category)
- CreatedAt (DateTime)
- UpdatedAt (DateTime, nullable)
- RecipeIngredients (Collection)

**Ingredient**

- Id (Guid, PK)
- Name (string, max 100)
- Unit (string, max 20) - "g", "ml", "pcs", etc.
- CreatedAt (DateTime)
- RecipeIngredients (Collection)

**RecipeIngredient** (Bridge Table)

- RecipeId (Guid, PK, FK → Recipe)
- IngredientId (Guid, PK, FK → Ingredient)
- Quantity (decimal)

**Category**

- Id (Guid, PK)
- Name (string, max 100)
- Recipes (Collection)

### Relationships

- User → Recipes (One-to-Many)
- Recipe → Category (Many-to-One)
- Recipe ↔ Ingredient (Many-to-Many via RecipeIngredient)

## 📡 API Endpoints

### Authentication

```
POST   /api/auth/register          # Register new user
POST   /api/auth/login             # Login user
```

### User Profile

```
GET    /api/profile                # Get current user profile
PUT    /api/profile                # Update profile (username, email, avatar)
PUT    /api/profile/password       # Change password
```

### Recipes

```
GET    /api/recipe                 # Get all user's recipes (with filters/sorting)
GET    /api/recipe/{id}            # Get single recipe with ingredients
POST   /api/recipe                 # Create new recipe
PUT    /api/recipe/{id}            # Update recipe
DELETE /api/recipe/{id}            # Delete recipe
POST   /api/recipe/{id}/image      # Upload recipe image
```

**Query Parameters:**

- `?searchTerm={text}` - Search recipes by title
- `?categoryId={guid}` - Filter by category
- `?ingredientName={text}` - Filter by ingredient
- `?sortBy={field}` - Sort by: title, createdDate, cookTime
- `?sortOrder={asc|desc}` - Sort direction

### Ingredients

```
GET    /api/ingredient             # Get all ingredients
GET    /api/ingredient/{id}        # Get single ingredient
POST   /api/ingredient             # Create ingredient
```

**Query Parameters:**

- `?searchTerm={text}` - Search ingredients by name

### Categories

```
GET    /api/category               # Get all categories with recipe counts
```

### Dashboard (VG Requirement)

```
GET    /api/dashboard/stats        # Get user statistics
```

**Response includes:**

- Total recipes count
- Recipes grouped by category
- Average cook time
- Average prep time
- 5 most recent recipes

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server Express (or SQL Server)
- Visual Studio 2022 / VS Code / Rider

### Backend Setup

1. **Clone the repository**

```bash
git clone https://github.com/yourusername/savory-backend.git
cd savory-backend
```

2. **Update connection string**
   Create `src/Api/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=SavoryDb;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Jwt": {
    "Key": "YourSecretKeyHereMinimum32CharactersLong!!!",
    "Issuer": "SavoryAPI",
    "Audience": "SavoryAPI",
    "ExpiresInMinutes": 60
  }
}
```

**3. Create database**

**Option A: Using migrations**

```bash
cd src/Api
dotnet ef database update
```

**Option B: Using SQL script**

```sql
-- In SSMS, create database
CREATE DATABASE SavoryDb;
GO

USE SavoryDb;
GO

-- Run savory-db-setup.sql script
```

**4. Run the API**

```bash
cd src/Api
dotnet run
```

API will be available at: `https://localhost:7286`

**5. Access Swagger UI**

```
https://localhost:7286/swagger
```

## 🧪 Testing

### Run All Tests

```bash
dotnet test
```

### Run Specific Test Projects

```bash
# Unit tests
dotnet test Tests/ApplicationTests

# Integration tests
dotnet test Tests/InfrastructureTests
```

### Test Coverage

**Unit Tests (11 total):**

- CreateRecipeCommandHandler - success & authentication failures
- UpdateRecipeCommandHandler - success & authorization failures
- DeleteRecipeCommandHandler - success & not found scenarios
- GetRecipeByIdQueryHandler - success & authorization failures
- AutoMapper configuration validation
- Entity mappings (Recipe, Ingredient, Category)

**Integration Tests (3 total):**

- Complete auth flow (register → login → access protected endpoint)
- Recipe CRUD operations (create → retrieve with full data)
- Authorization enforcement (users cannot access other users' recipes)

**CI/CD:**

- All tests run automatically on push/PR via GitHub Actions
- Tests run in isolated in-memory database
- Code formatting validation with dotnet format

## 🐛 Known Issues

- No pagination on recipe lists (can be added if performance becomes an issue)
- Images stored locally in wwwroot (would need cloud storage for production deployment)
- No recipe sharing between users
- Category management is read-only (seeded data only)

## 🔮 Future Improvements

**Technical Improvements:**

- Add pagination to recipe and ingredient lists
- Migrate image storage to Cloudinary/Azure Blob Storage
- Implement caching for frequently accessed data (Redis)
- Add rate limiting on API endpoints
- Implement soft delete for recipes
- Add recipe versioning (track changes)

**Feature Improvements:**

- Meal planning calendar
- Shopping list generation from recipes
- Recipe sharing between users
- Import recipes from URLs
- Nutritional information calculation
- Recipe ratings and reviews
- Multi-language support
- Recipe print view

## 📚 Reflection

### Architecture Design

I chose Clean Architecture with CQRS because it provides clear separation between business logic and infrastructure concerns. This made the codebase more maintainable and testable. The CQRS pattern with MediatR was initially challenging but ultimately made the code more organized - each operation has its own handler, validator, and clear responsibility.

The decision to use the Repository pattern paid off during testing, as I could easily mock data access. However, I learned that the pattern can sometimes feel like unnecessary abstraction for simple CRUD operations. In a real project, I might use repositories only where complex queries are needed.

### Key Learnings

**Technical Skills:**

- **Identity & JWT:** ASP.NET Core Identity was more complex than expected, especially configuring proper password requirements and JWT claims. Understanding the relationship between UserManager, SignInManager, and JWT generation took time but gave me solid authentication knowledge.

- **CQRS with MediatR:** Initially felt like overkill, but the pattern really shines as the application grows. Each command/query is self-contained with its own validation, making the code predictable and easy to test.

- **Testing Strategy:** I learned the hard way that writing tests after implementation is harder than TDD. The integration tests particularly highlighted configuration issues (like JWT settings in CI) that I wouldn't have caught otherwise.

- **Entity Framework Relationships:** The many-to-many relationship with payload (RecipeIngredient with Quantity) required careful configuration. Learning about tracked entities and when to use `.Clear()` vs manual removal was crucial for the update operations.

### Challenges Faced

**1. Constructor Issues with JSON Deserialization:**
Early on, I added constructors to my commands thinking it was good OOP practice. This broke JSON deserialization in ASP.NET Core! Learning that commands should be POCOs (property initialization only) was a valuable lesson about framework conventions.

**2. Repository Method Selection:**
Using `GetByIdAsync()` vs `GetRecipeWithDetailsAsync()` caused duplicate key errors in updates. I learned that navigation properties must be loaded (via Include) for `.Clear()` to properly mark items for deletion. This taught me about EF Core change tracking.

**3. CI/CD Configuration:**
Integration tests failed in GitHub Actions because JWT configuration wasn't available. Solving this taught me about different configuration strategies (in-memory config, test-specific settings, environment-based configuration).

**4. Clean Architecture Boundaries:**
I almost had my GetCategoriesQueryHandler directly inject ApplicationDbContext, which would violate Clean Architecture (Application → Infrastructure dependency). Catching this and moving the logic to a repository method reinforced the importance of maintaining proper layer boundaries.

### What I Would Do Differently

**1. Start with TDD:** Writing tests first would have caught the update recipe bug earlier and forced better design decisions.

**2. More Comprehensive Logging Earlier:** I added Serilog late in the project. Having proper logging from day one would have made debugging much faster.

**3. API Versioning from Start:** While not needed now, adding API versioning from the beginning would make future breaking changes easier to manage.

**4. More Granular Commits:** Some of my commits covered too many changes. Smaller, focused commits would make the history more useful.

**5. Consider Specification Pattern:** For complex filtering, the Specification pattern might be cleaner than passing multiple optional parameters to repository methods.

### Conclusion

This project gave me hands-on experience with production-quality .NET development. The combination of Clean Architecture, CQRS, comprehensive testing, and CI/CD creates a solid foundation that could scale to a real application. The most valuable takeaway is understanding how architectural patterns work together - how Clean Architecture enables testing, how CQRS simplifies complex operations, and how proper separation of concerns makes code maintainable.

The VG requirements (filtering, sorting, dashboard, integration tests) pushed me beyond basic CRUD to think about real application needs. Building features that actually improve user experience - like search, categorization, and statistics - made the project feel like something I'd actually want to use.

## 👤 Author

**Gabby Ferm**

- GitHub: [@GabbyFerm](https://github.com/GabbyFerm)
- Email: gabbzf@gmail.com

## 📄 License

This project is for educational purposes as part of coursework at [Your School Name].

---

**Project Status:** Backend Complete ✅ | Frontend In Progress 🚧
