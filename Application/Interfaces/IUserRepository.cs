using Domain.Aggregates;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IUserRepository : IAsyncRepository<UserAggregate>
    {
        Task<UserAggregate> GetByUsernameAsync(string username);
        Task<IdentityResult> RegisterUserAsync(UserAggregate user, string password);
        Task<bool> CheckPasswordAsync(UserAggregate user, string password);
    }
}