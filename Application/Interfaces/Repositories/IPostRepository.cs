using Domain.Aggregates;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IPostRepository : IAsyncRepository<Post>
    {
        Task<IReadOnlyList<Post>> GetAllUserPostsAsync(int userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Post>> GetAllFollowedUsersPostsAsync(List<int> userIds, CancellationToken cancellationToken = default);
        Task<Comment?> GetCommentByIdAsync(int commentId, CancellationToken cancellationToken = default);
        Task CreateCommentAsync(Comment comment, CancellationToken cancellationToken = default);
        Task DeleteCommentAsync(Comment comment, CancellationToken cancellationToken = default);
    }
}
