using Domain.Aggregates;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IWorkoutRepository : IAsyncRepository<Workout>
    {
        Task<IReadOnlyList<Workout>> GetUserWorkoutsAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Exercise>> GetExercisesAsync(CancellationToken cancellationToken = default);
        Task CreateExerciseAsync(Exercise exercise, CancellationToken cancellationToken = default);
    }
}