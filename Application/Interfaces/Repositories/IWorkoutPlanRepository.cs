using Domain.Aggregates;

namespace Application.Interfaces.Repositories
{
    public interface IWorkoutPlanRepository : IRepository<WorkoutPlan>
    {
        Task<WorkoutPlan?> GetUserCurrentWorkoutPlanAsync(int id, CancellationToken cancellationToken = default);
    }
}
