# 🍽️ Savory - Personal Recipe Manager

A fullstack recipe management application where users can store, organize, and manage their personal recipes with ease.

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

Savory is a personal recipe manager built with .NET and React. Users can create accounts, add their favorite recipes with ingredients, organize them by categories, and manage their personal cookbook digitally.

**Project Purpose:** School assignment for Objectoriented Programming - Advanced

## 🛠️ Tech Stack

**Backend:**
- .NET 8 Web API
- Entity Framework Core
- SQL Server (SSMS)
- ASP.NET Core Identity
- AutoMapper
- xUnit + Moq

**Frontend:**
- React 18
- React Router
- Axios
- CSS Modules / Tailwind CSS

**DevOps:**
- GitHub Actions (CI/CD)
- Git version control

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:
```
┌─────────────────────────────────────────┐
│          Savory.Api (Controllers)       │
├─────────────────────────────────────────┤
│   Savory.Application (Services, DTOs)   │
├─────────────────────────────────────────┤
│    Savory.Core (Domain Models, IRepo)   │
├─────────────────────────────────────────┤
│  Savory.Infrastructure (EF, Identity)   │
└─────────────────────────────────────────┘
```

**Key Patterns:**
- Repository Pattern
- Service Layer Pattern
- Dependency Injection
- DTO Pattern for API responses

## ✨ Features

### Core Features 
- ✅ User registration and authentication with Identity
- ✅ User profile management (username, email, password)
- ✅ Personalized user avatars with initials
- ✅ Customizable avatar colors (preset palette)
- ✅ Create, read, update, delete recipes
- ✅ Add ingredients to recipes with quantities
- ✅ Organize recipes by categories
- ✅ Upload recipe images
- ✅ View personal recipe collection
- ✅ Global error handling
- ✅ Input validation

### Advanced Features 
- ⭐ Server-side filtering by category and search term
- ⭐ Server-side sorting (by date, title, cook time)
- ⭐ Dashboard with statistics (total recipes, by category, avg cook time)
- ⭐ Custom exceptions with structured error responses
- ⭐ React Context for state management
- ⭐ Search and filter UI

## 🗄️ Database Schema

### Core Models

**User** (via Identity)
- Id (Guid)
- Email (string)
- Username (string)
- PasswordHash (string)
- AvatarColor (string) - Hex color for avatar background
- Recipes (Collection)

**Recipe**
- Id (Guid)
- UserId (Guid, FK)
- Title (string)
- Description (string)
- Instructions (string)
- PrepTime (int, minutes)
- CookTime (int, minutes)
- Servings (int)
- ImagePath (string, nullable)
- CategoryId (Guid, FK)
- CreatedAt (DateTime)
- RecipeIngredients (Collection)

**Ingredient**
- Id (Guid)
- Name (string)
- Unit (string, e.g., "g", "ml", "pcs")
- RecipeIngredients (Collection)

**RecipeIngredient** (Bridge table)
- RecipeId (Guid, FK)
- IngredientId (Guid, FK)
- Quantity (decimal)

**Category**
- Id (Guid)
- Name (string)
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
POST   /api/auth/logout            # Logout user
```

### User Profile
```
GET    /api/profile                # Get current user profile
PUT    /api/profile                # Update profile (username, email)
PUT    /api/profile/password       # Change password
```

### Recipes
```
GET    /api/recipes                # Get all user's recipes (with filtering/sorting)
GET    /api/recipes/{id}           # Get single recipe with ingredients
POST   /api/recipes                # Create new recipe
PUT    /api/recipes/{id}           # Update recipe
DELETE /api/recipes/{id}           # Delete recipe
POST   /api/recipes/{id}/image     # Upload recipe image
```

### Ingredients
```
GET    /api/ingredients            # Get all ingredients
GET    /api/ingredients/{id}       # Get single ingredient
POST   /api/ingredients            # Create ingredient
```

### Categories
```
GET    /api/categories             # Get all categories
```

### Dashboard 
```
GET    /api/dashboard/stats        # Get user statistics
```

**Query Parameters:**
- `?search={term}` - Search recipes by title
- `?categoryId={guid}` - Filter by category
- `?sortBy={field}` - Sort by: title, createdDate, cookTime
- `?sortOrder={asc|desc}` - Sort direction

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (SSMS)
- Node.js (v18+)
- npm or yarn

### Backend Setup

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/savory-backend.git
cd savory-backend
```

2. **Update connection string**
Edit `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=SavoryDb;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

3. **Create database**
```bash
cd Savory.Api
dotnet ef database update
```

Or use the provided SQL script:
```bash
# Run savory-db-setup.sql in SSMS
```

4. **Run the API**
```bash
dotnet run --project Savory.Api
```

API will be available at: `https://localhost:5001`

### Frontend Setup

1. **Clone the frontend repository**
```bash
git clone https://github.com/yourusername/savory-frontend.git
cd savory-frontend
```

2. **Install dependencies**
```bash
npm install
```

3. **Configure API URL**
Create `.env` file:
```
REACT_APP_API_URL=https://localhost:5001/api
```

4. **Run the app**
```bash
npm start
```

App will be available at: `http://localhost:3000`

## 🧪 Testing

### Run Unit Tests
```bash
dotnet test Savory.Tests
```

### Test Coverage
- ✅ 8+ unit tests (Services, business logic)
- ✅ 3+ integration tests (API endpoints)

**Key test areas:**
- Recipe creation and validation
- User authorization (users can only modify own recipes)
- Ingredient management
- Search and filtering logic
- Edge cases (null values, invalid data)

### Example Tests
- `RecipeService_CreateRecipe_ShouldAddRecipeToDatabase`
- `RecipeService_DeleteRecipe_WhenNotOwner_ShouldThrowUnauthorizedException`
- `RecipeController_GetRecipes_WithFilter_ShouldReturnFilteredResults`

## 🐛 Known Issues

- Image upload is local only (stored in wwwroot/images)
- No pagination on ingredients list
- Category management is read-only for users

## 🔮 Future Improvements

**Technical improvements:**
- Migrate image storage to Cloudinary for deployment
- Add pagination to recipe lists
- Implement caching for frequently accessed data
- Add rate limiting on API endpoints

**Feature improvements:**
- Meal planning calendar
- Shopping list generation from recipes
- Recipe sharing between users
- Import recipes from URLs
- Nutritional information

**What I would do differently next time:**
- Start with TDD from day one (wrote some tests after implementation)
- Use AutoMapper profiles from the beginning
- Implement CQRS pattern for clearer separation
- Add more comprehensive logging with Serilog
- Consider NoSQL for recipe storage (more flexible schema)

## 📚 Reflection

### Architecture Design
I chose Clean Architecture because it provides clear separation between business logic and infrastructure concerns. This made testing easier and the codebase more maintainable. The Service-Repository pattern helped keep controllers thin and focused on HTTP concerns.

### Key Learnings
- Identity setup was more complex than expected, but provides robust security
- TDD helped catch edge cases early (especially around user authorization)
- AutoMapper significantly reduced boilerplate code
- Server-side filtering required careful LINQ query construction

### Challenges Faced
- Handling many-to-many relationships with quantities (RecipeIngredient bridge table)
- File upload implementation and path management
- React state management with authentication context

## 👤 Author

**Gabby Ferm**
- GitHub: [@GabbyFerm](https://github.com/GabbyFerm)
- Email: gabbzf@gmail.com

## 📄 License

This project is for educational purposes.
