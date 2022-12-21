using Domain.Aggregates;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPostRepository : IAsyncRepository<Post>
    {
        Task<IReadOnlyList<Post>> GetAllUserPostsAsync(int id);
        Task<IReadOnlyList<Post>> GetAllFollowedUsersPostsAsync(List<int> ids);
        Task CreateCommentAsync(Comment comment);
    }
}