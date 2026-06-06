using System.Security.Claims;

namespace WebAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        } 
    }
}