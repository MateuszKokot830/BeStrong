using Domain.Aggregates;

namespace Application.Interfaces.Repositories
{
    public interface IWorkoutPlanRepository : IAsyncRepository<WorkoutPlan>
    {
        Task<WorkoutPlan?> GetUserCurrentWorkoutPlanAsync(int id);
    }
}