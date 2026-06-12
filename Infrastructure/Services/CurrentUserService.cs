using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public int UserId =>
            int.TryParse(
                _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var id)
            ? id
            : 0;

        public bool IsAdmin =>
            _httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;

        public bool IsOwnerOrAdmin(int resourceOwnerId) =>
            IsAdmin || UserId == resourceOwnerId;
    }
}
