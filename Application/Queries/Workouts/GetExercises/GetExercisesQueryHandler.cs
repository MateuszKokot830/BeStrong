using Application.Dto.Exercise;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetExercises
{
    public class GetExercisesQueryHandler(IWorkoutRepository workoutRepository)
        : IRequestHandler<GetExercisesQuery, ErrorOr<IEnumerable<ExerciseDto>>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;

        public async Task<ErrorOr<IEnumerable<ExerciseDto>>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
        {
            var exercises = await _workoutRepository.GetExercisesAsync(cancellationToken);
            return exercises.Select(e => e.ToDto()).ToList();
        }
    }
}
