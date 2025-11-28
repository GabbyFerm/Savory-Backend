using Application.DTOs;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Tests.InfrastructureTests;

/// <summary>
/// Integration tests for Savory API endpoints
/// </summary>
public class ApiIntegrationTests : IntegrationTestBase
{
    public ApiIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task AuthFlow_RegisterLoginAndAccessProtectedEndpoint_ShouldSucceed()
    {
        // Arrange
        var userName = "testuser";
        var email = "test@example.com";
        var password = "Test123";

        // Act - Register
        var registerResponse = await RegisterUserAsync(userName, email, password);

        // Assert - Registration successful
        registerResponse.Should().NotBeNull();
        registerResponse.Token.Should().NotBeNullOrEmpty();
        registerResponse.User.Should().NotBeNull();
        registerResponse.User.UserName.Should().Be(userName);
        registerResponse.User.Email.Should().Be(email);

        // Act - Login
        var loginResponse = await LoginUserAsync(email, password);

        // Assert - Login successful
        loginResponse.Should().NotBeNull();
        loginResponse.Token.Should().NotBeNullOrEmpty();
        loginResponse.User.UserName.Should().Be(userName);

        // Act - Access protected endpoint
        SetAuthorizationHeader(loginResponse.Token);
        var recipesResponse = await Client.GetAsync("/api/recipe");

        // Assert - Can access protected endpoint
        recipesResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RecipeCRUD_CreateAndRetrieveRecipe_ShouldSucceed()
    {
        // Arrange - Register and login
        var authResponse = await RegisterUserAsync("recipeuser", "recipe@example.com", "Recipe123");
        SetAuthorizationHeader(authResponse.Token);

        // Get categories to use valid category ID
        var categoriesResponse = await Client.GetAsync("/api/category");
        var categories = await categoriesResponse.Content.ReadFromJsonAsync<List<CategoryWithCountDto>>();
        var categoryId = categories!.First().Id;

        // Get ingredients to use valid ingredient ID
        var ingredientsResponse = await Client.GetAsync("/api/ingredient");
        var ingredients = await ingredientsResponse.Content.ReadFromJsonAsync<List<IngredientDto>>();
        var ingredientId = ingredients!.First().Id;

        // Act - Create recipe
        var createRecipeRequest = new
        {
            title = "Test Recipe",
            description = "Test Description",
            instructions = "Test Instructions",
            prepTime = 10,
            cookTime = 20,
            servings = 4,
            categoryId,
            ingredients = new[]
            {
                new { ingredientId, quantity = 100 }
            }
        };

        var createResponse = await Client.PostAsJsonAsync("/api/recipe", createRecipeRequest);

        // Assert - Recipe created
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdRecipe = await createResponse.Content.ReadFromJsonAsync<RecipeDto>();
        createdRecipe.Should().NotBeNull();
        createdRecipe!.Title.Should().Be("Test Recipe");
        createdRecipe.Ingredients.Should().HaveCount(1);

        // Act - Get recipe by ID
        var getResponse = await Client.GetAsync($"/api/recipe/{createdRecipe.Id}");

        // Assert - Recipe retrieved
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedRecipe = await getResponse.Content.ReadFromJsonAsync<RecipeDto>();
        retrievedRecipe.Should().NotBeNull();
        retrievedRecipe!.Title.Should().Be("Test Recipe");
        retrievedRecipe.Description.Should().Be("Test Description");
    }

    [Fact]
    public async Task RecipeAuthorization_UserCannotAccessOtherUsersRecipes_ShouldReturnNotFound()
    {
        // Arrange - User A creates a recipe
        var userA = await RegisterUserAsync("usera", "usera@example.com", "UserA123");
        SetAuthorizationHeader(userA.Token);

        var categoriesResponse = await Client.GetAsync("/api/category");
        var categories = await categoriesResponse.Content.ReadFromJsonAsync<List<CategoryWithCountDto>>();
        var categoryId = categories!.First().Id;

        var ingredientsResponse = await Client.GetAsync("/api/ingredient");
        var ingredients = await ingredientsResponse.Content.ReadFromJsonAsync<List<IngredientDto>>();
        var ingredientId = ingredients!.First().Id;

        var createRecipeRequest = new
        {
            title = "User A Recipe",
            description = "Private recipe",
            instructions = "Secret instructions",
            prepTime = 5,
            cookTime = 10,
            servings = 2,
            categoryId,
            ingredients = new[]
            {
                new { ingredientId, quantity = 50 }
            }
        };

        var createResponse = await Client.PostAsJsonAsync("/api/recipe", createRecipeRequest);
        var userARecipe = await createResponse.Content.ReadFromJsonAsync<RecipeDto>();

        // Act - User B tries to access User A's recipe
        var userB = await RegisterUserAsync("userb", "userb@example.com", "UserB123");
        SetAuthorizationHeader(userB.Token);

        var getResponse = await Client.GetAsync($"/api/recipe/{userARecipe!.Id}");

        // Assert - User B cannot access User A's recipe
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}