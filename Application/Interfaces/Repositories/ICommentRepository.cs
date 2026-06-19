using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Comment comment, CancellationToken cancellationToken = default);
        Task DeleteAsync(Comment comment, CancellationToken cancellationToken = default);
    }
}
