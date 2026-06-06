using Domain.Aggregates;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IPostRepository : IAsyncRepository<Post>
    {
        Task<IReadOnlyList<Post>> GetAllUserPostsAsync(int userId);
        Task<IReadOnlyList<Post>> GetAllFollowedUsersPostsAsync(List<int> userIds);
        Task CreateCommentAsync(Comment comment);
        Task DeleteCommentAsync(Comment comment);
    }
}