using Application.Dto.WorkoutPlan;

namespace Application.Interfaces.Searchers
{
    public interface IWorkoutPlanSearcher
    {
        Task<WorkoutPlanDto?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<WorkoutPlanDto>> GetPublicAsync(CancellationToken cancellationToken = default);
    }
}
