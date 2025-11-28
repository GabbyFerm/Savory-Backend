using Application.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Tests.InfrastructureTests;

/// <summary>
/// Base class for integration tests with helper methods
/// </summary>
public class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient Client;

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        Client = factory.CreateClient();
    }

    /// <summary>
    /// Registers a new user and returns authentication response
    /// </summary>
    protected async Task<AuthResponseDto> RegisterUserAsync(string userName, string email, string password)
    {
        var registerRequest = new
        {
            userName,
            email,
            password
        };

        var response = await Client.PostAsJsonAsync("/api/auth/register", registerRequest);
        response.EnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        return authResponse!;
    }

    /// <summary>
    /// Logs in a user and returns authentication response
    /// </summary>
    protected async Task<AuthResponseDto> LoginUserAsync(string email, string password)
    {
        var loginRequest = new
        {
            email,
            password
        };

        var response = await Client.PostAsJsonAsync("/api/auth/login", loginRequest);
        response.EnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        return authResponse!;
    }

    /// <summary>
    /// Sets authorization header with JWT token
    /// </summary>
    protected void SetAuthorizationHeader(string token)
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    /// <summary>
    /// Clears authorization header
    /// </summary>
    protected void ClearAuthorizationHeader()
    {
        Client.DefaultRequestHeaders.Authorization = null;
    }
}