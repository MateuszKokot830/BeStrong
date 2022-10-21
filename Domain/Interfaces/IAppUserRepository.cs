using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAppUserRepository
    {
        Task<IEnumerable<AppUser>> GetAll();
        Task<AppUser> GetById(int id);
        void Add(AppUser appUser);
        void Update(AppUser appUser);
        void Delete(AppUser appUser);
    }
}