using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAppUserRepository : IAsyncRepository<AppUser>
    {
        Task<AppUser> GetByUsername(string username);
    }
}