using Application.Dto.Workout;
using Application.Helpers;
using Application.Helpers.Criteria;

namespace Application.Interfaces.Searchers
{
    public interface IWorkoutSearcher
    {
        Task<IReadOnlyList<WorkoutDto>> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<PaginationList<WorkoutDto>> GetPagedAsync(WorkoutSearchCriteria criteria, int userId, CancellationToken cancellationToken = default);
    }
}
