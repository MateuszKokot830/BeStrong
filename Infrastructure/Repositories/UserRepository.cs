using Domain.Aggregates;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<UserAggregate>, IUserRepository
    {
        private readonly UserManager<UserAggregate> _userManager;
        public UserRepository(DataContext context, UserManager<UserAggregate> userManager) : base(context)
        {
            _userManager = userManager;
        }
        public async Task<UserAggregate> GetByUsernameAsync(string username)
        {
            return await _userManager.Users.SingleOrDefaultAsync(x=>x.UserName == username.ToLower());
        }

        public async Task<IdentityResult> RegisterUserAsync(UserAggregate user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<bool> CheckPasswordAsync(UserAggregate user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}