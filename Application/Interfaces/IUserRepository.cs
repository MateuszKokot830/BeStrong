using Domain.Aggregates;

namespace Application.Interfaces
{
    public interface IUserRepository : IAsyncRepository<UserAggregate>
    {
        Task<UserAggregate> GetByUsername(string username);
    }
}