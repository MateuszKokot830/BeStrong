using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {

        public IEnumerable<AppUser> GetAll()
        {
            throw new NotImplementedException();
        }

        public AppUser GetById(int id)
        {
            throw new NotImplementedException();
        }

        public AppUser Add(AppUser appUser)
        {
            throw new NotImplementedException();
        }

        public void Update(AppUser appUser)
        {
            throw new NotImplementedException();
        }
        
        public void Delete(AppUser appUser)
        {
            throw new NotImplementedException();
        }
    }
}