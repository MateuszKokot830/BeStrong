using Domain.Aggregates;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        public UserRepository(DataContext context, UserManager<User> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public override async Task<IReadOnlyList<User>> GetAllAsync()
        {
            return await _userManager.Users.Include(p => p.Photos).Include(p => p.Measurements)
                .ToListAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userManager.Users.Include(p => p.Photos).Include(p => p.Measurements)
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
    }
}