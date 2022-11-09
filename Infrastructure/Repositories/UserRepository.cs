using Domain.Aggregates;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<UserAggregate>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public async Task<UserAggregate> GetByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(x=>x.Username == username.ToLower());
        }
        
    }
}