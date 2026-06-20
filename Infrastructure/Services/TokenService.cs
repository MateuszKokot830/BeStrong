using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Dto.Auth;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class TokenService(
    UserManager<User> userManager,
    IConfiguration configuration) : ITokenService
{
    public async Task<string?> CreateTokenAsync(CreateTokenRequest request, CancellationToken cancellationToken = default)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, request.UserId.ToString()),
            new(ClaimTypes.Name, request.Username)
        };

        var identityUser = await userManager.FindByNameAsync(request.Username);
        if (identityUser != null)
        {
            var roles = await userManager.GetRolesAsync(identityUser);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"] ?? configuration["TokenKey"] ?? string.Empty));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddMinutes(double.Parse(configuration["Jwt:ExpirationInMinutes"] ?? "60"));

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"] ?? "BeStrong",
            audience: configuration["Jwt:Audience"] ?? "BeStrongUsers",
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
