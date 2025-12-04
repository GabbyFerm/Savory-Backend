using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}