using Application.Dto.Exercise;

namespace Application.Interfaces.Searchers
{
    public interface IExerciseSearcher
    {
        Task<IReadOnlyList<ExerciseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
