using Domain.Aggregates;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task AddLikeAsync(PostLike like, CancellationToken cancellationToken = default);
        Task DeleteLikeAsync(PostLike like, CancellationToken cancellationToken = default);
    }
}
