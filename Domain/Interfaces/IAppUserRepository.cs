using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAppUserRepository
    {
        Task<IEnumerable<AppUser>> GetAll();
        Task<AppUser> GetById(int id);
        Task<AppUser> GetByUsername(string username);
        Task Add(AppUser appUser);
        Task Update(AppUser appUser);
        Task Delete(AppUser appUser);
    }
}