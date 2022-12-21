using Domain.Aggregates;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPostRepository : IAsyncRepository<Post>
    {
        Task CreateCommentAsync(Comment comment);
    }
}