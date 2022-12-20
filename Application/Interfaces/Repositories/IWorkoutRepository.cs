using Domain.Aggregates;

namespace Application.Interfaces
{
    public interface IWorkoutRepository : IAsyncRepository<Workout>
    {
        Task<IReadOnlyList<Workout>> GetUserWorkoutsAsync(int id);
    }
}