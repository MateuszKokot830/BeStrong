using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private DataContext _context;
        public AppUserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppUser>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(x=>x.Username == username.ToLower());
        }

        public async Task Add(AppUser appUser)
        {
            _context.Users.Add(appUser); 
            await _context.SaveChangesAsync();
        }

        public async Task Update(AppUser appUser)
        {
            _context.Users.Update(appUser);
            await _context.SaveChangesAsync();
        }
        
        public async Task Delete(AppUser appUser)
        {
            _context.Users.Remove(appUser);
            await _context.SaveChangesAsync();
        }
    }
}