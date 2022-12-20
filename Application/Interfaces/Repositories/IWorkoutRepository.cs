using Domain.Aggregates;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IWorkoutRepository : IAsyncRepository<Workout>
    {
        Task<IReadOnlyList<Workout>> GetUserWorkoutsAsync(int id);
        Task<IReadOnlyList<Exercise>> GetExercisesAsync();
    }
}