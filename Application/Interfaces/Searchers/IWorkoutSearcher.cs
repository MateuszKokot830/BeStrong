using Application.Dto.Workout;

namespace Application.Interfaces.Searchers
{
    public interface IWorkoutSearcher
    {
        Task<IReadOnlyList<WorkoutDto>> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    }
}
