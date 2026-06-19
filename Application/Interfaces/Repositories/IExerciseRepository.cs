using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IExerciseRepository
    {
        Task AddAsync(Exercise exercise, CancellationToken cancellationToken = default);
    }
}
