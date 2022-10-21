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

        public void Add(AppUser appUser)
        {
            _context.Users.Add(appUser);
            _context.SaveChanges();
        }

        public void Update(AppUser appUser)
        {
            _context.Users.Update(appUser);
            _context.SaveChanges();
        }
        
        public void Delete(AppUser appUser)
        {
            _context.Users.Remove(appUser);
            _context.SaveChanges();
        }
    }
}