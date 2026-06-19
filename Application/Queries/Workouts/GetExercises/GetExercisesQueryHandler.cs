using Application.Dto.Exercise;
using Application.Interfaces.Searchers;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetExercises
{
    public class GetExercisesQueryHandler(IExerciseSearcher exerciseSearcher)
        : IRequestHandler<GetExercisesQuery, ErrorOr<IEnumerable<ExerciseDto>>>
    {
        private readonly IExerciseSearcher _exerciseSearcher = exerciseSearcher;

        public async Task<ErrorOr<IEnumerable<ExerciseDto>>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
        {
            var exercises = await _exerciseSearcher.GetAllAsync(cancellationToken);
            return exercises.ToList();
        }
    }
}
