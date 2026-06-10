using Domain.Aggregates;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Application.Interfaces.Repositories;

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
            .Include(u => u.Followers);

        public async Task<User?> GetByUsernameAsync(string? username, CancellationToken cancellationToken = default)
        {
            var usernameNormalized = username?.ToUpperInvariant() ?? string.Empty;
            return await GetQueryable()
                .SingleOrDefaultAsync(x => x.NormalizedUserName == usernameNormalized, cancellationToken);
        }

        public async Task<IdentityResult> RegisterUserAsync(User user, string? password, CancellationToken cancellationToken = default)
        {
            var pw = password ?? string.Empty;
            return await _userManager.CreateAsync(user, pw);
        }

        public async Task<bool> CheckPasswordAsync(User user, string? password, CancellationToken cancellationToken = default)
        {
            var pw = password ?? string.Empty;
            return await _userManager.CheckPasswordAsync(user, pw);
        }

        public async Task AddFollowerAsync(Follower follower, CancellationToken cancellationToken = default)
        {
            _context.Followers.Add(follower);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteFollowerAsync(Follower follower, CancellationToken cancellationToken = default)
        {
            _context.Followers.Remove(follower);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddPhoto(Photo photo, CancellationToken cancellationToken = default)
        {
            _context.Photos.Add(photo);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeletePhoto(Photo photo, CancellationToken cancellationToken = default)
        {
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
