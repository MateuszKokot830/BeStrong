using Application.Dto.WorkoutPlan;
using Application.Helpers;
using Application.Helpers.Criteria;

namespace Application.Interfaces.Searchers
{
    public interface IWorkoutPlanSearcher
    {
        Task<WorkoutPlanDto?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PaginationList<WorkoutPlanDto>> GetPagedAsync(WorkoutPlanSearchCriteria criteria, int requestingUserId, CancellationToken cancellationToken = default);
    }
}
