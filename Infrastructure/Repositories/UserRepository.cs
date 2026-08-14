using Application.Interfaces.Repositories;
using Domain.Aggregates;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository(DataContext context, UserManager<User> userManager)
        : BaseRepository<User>(context), IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;

        protected override IQueryable<User> GetQueryable() => _userManager.Users
            .Include(u => u.Photos)
            .Include(u => u.Measurements)
            .Include(u => u.FollowedUsers)
            .Include(u => u.Followers)
            .Include(u => u.WorkoutPlan);

        public async Task<User?> GetByUsernameAsync(string? username, CancellationToken cancellationToken = default)
        {
            var normalized = username?.ToUpperInvariant() ?? string.Empty;
            return await GetQueryable()
                .SingleOrDefaultAsync(u => u.NormalizedUserName == normalized, cancellationToken);
        }

        public Task<IdentityResult> RegisterUserAsync(User user, string? password, CancellationToken cancellationToken = default) =>
            _userManager.CreateAsync(user, password ?? string.Empty);

        public Task<bool> CheckPasswordAsync(User user, string? password, CancellationToken cancellationToken = default) =>
            _userManager.CheckPasswordAsync(user, password ?? string.Empty);

        public Task AddFollowerAsync(Follower follower, CancellationToken cancellationToken = default)
        {
            _context.Followers.Add(follower);
            return Task.CompletedTask;
        }

        public Task DeleteFollowerAsync(Follower follower, CancellationToken cancellationToken = default)
        {
            _context.Followers.Remove(follower);
            return Task.CompletedTask;
        }

        public Task AddPhotoAsync(Photo photo, CancellationToken cancellationToken = default)
        {
            _context.Photos.Add(photo);
            return Task.CompletedTask;
        }

        public Task DeletePhotoAsync(Photo photo, CancellationToken cancellationToken = default)
        {
            _context.Photos.Remove(photo);
            return Task.CompletedTask;
        }
    }
}
