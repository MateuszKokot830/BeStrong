using Application.Dto.Post;

namespace Application.Interfaces.Searchers
{
    public interface IPostSearcher
    {
        Task<IReadOnlyList<PostDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PostDto>> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PostDto>> FindByUserIdsAsync(IReadOnlyCollection<int> userIds, CancellationToken cancellationToken = default);
    }
}
