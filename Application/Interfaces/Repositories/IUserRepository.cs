using Domain.Aggregates;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<IdentityResult> RegisterUserAsync(User user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}