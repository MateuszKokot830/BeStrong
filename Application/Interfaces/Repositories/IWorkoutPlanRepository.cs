using Domain.Aggregates;

namespace Application.Interfaces
{
    public interface IWorkoutPlanRepository : IAsyncRepository<WorkoutPlan>
    {
        Task<WorkoutPlan> GetUserCurrentWorkoutPlanAsync(int id);
    }
}