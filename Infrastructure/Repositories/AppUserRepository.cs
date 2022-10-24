using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AppUserRepository : BaseRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(DataContext context) : base(context)
        {
        }

        public async Task<AppUser> GetByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(x=>x.Username == username.ToLower());
        }
        
    }
}