using Domain.Aggregates;

namespace Application.Interfaces
{
    public interface IPostRepository : IAsyncRepository<Post>
    {
        
    }
}