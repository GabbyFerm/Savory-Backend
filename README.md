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

## 👤 Author

**Gabby Ferm**

- GitHub: [@GabbyFerm](https://github.com/GabbyFerm)
- Email: gabbzf@gmail.com

## 📄 License

This project is for educational purposes as part of coursework at [Your School Name].

---

**Built with ❤️ using .NET 8 and Clean Architecture.**