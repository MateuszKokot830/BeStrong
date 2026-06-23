using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IExerciseRepository
    {
        Task<Exercise?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Exercise exercise, CancellationToken cancellationToken = default);
        Task UpdateAsync(Exercise exercise, CancellationToken cancellationToken = default);
        Task DeleteAsync(Exercise exercise, CancellationToken cancellationToken = default);
    }
}
