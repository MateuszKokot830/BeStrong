using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAppUserRepository
    {
        IEnumerable<AppUser> GetAll();
        AppUser GetById(int id);
        AppUser Add(AppUser appUser);
        void Update(AppUser appUser);
        void Delete(AppUser appUser);
    }
}