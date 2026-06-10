using Domain.Aggregates;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User?> GetByUsernameAsync(string? username, CancellationToken cancellationToken = default);
        Task<IdentityResult> RegisterUserAsync(User user, string? password, CancellationToken cancellationToken = default);
        Task<bool> CheckPasswordAsync(User user, string? password, CancellationToken cancellationToken = default);
        Task AddFollowerAsync(Follower follower, CancellationToken cancellationToken = default);
        Task DeleteFollowerAsync(Follower follower, CancellationToken cancellationToken = default);
        Task AddPhoto(Photo photo, CancellationToken cancellationToken = default);
        Task DeletePhoto(Photo photo, CancellationToken cancellationToken = default);
    }
}
