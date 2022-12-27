using Domain.Aggregates;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        public UserRepository(DataContext context, UserManager<User> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _userManager.Users
            .Include(u => u.Photos)
            .Include(u => u.Measurements)
            .Include(u => u.FollowedUsers)
            .Include(u => u.Followers)
            .SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IReadOnlyList<User>> GetAllAsync()
        {
            return await _userManager.Users
            .Include(u => u.Photos)
            .Include(u => u.Measurements)
            .Include(u => u.FollowedUsers)
            .Include(u => u.Followers)
            .ToListAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userManager.Users
            .Include(u => u.Photos)
            .Include(u => u.Measurements)
            .Include(u => u.FollowedUsers)
            .Include(u => u.Followers)
            .SingleOrDefaultAsync(x=>x.UserName == username.ToLower());
        }

        public async Task<IdentityResult> RegisterUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task AddFollowerAsync(Follower follower) 
        {
            _context.Followers.Add(follower);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFollowerAsync(Follower follower) 
        {
            _context.Followers.Remove(follower);
            await _context.SaveChangesAsync();
        }
    }
}