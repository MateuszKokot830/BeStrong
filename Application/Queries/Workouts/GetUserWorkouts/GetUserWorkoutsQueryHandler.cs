using Application.Dto.Workout;
using Application.Interfaces.Searchers;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetUserWorkouts
{
    public class GetUserWorkoutsQueryHandler(IWorkoutSearcher workoutSearcher)
        : IRequestHandler<GetUserWorkoutsQuery, ErrorOr<IEnumerable<WorkoutDto>>>
    {
        private readonly IWorkoutSearcher _workoutSearcher = workoutSearcher;

        public async Task<ErrorOr<IEnumerable<WorkoutDto>>> Handle(GetUserWorkoutsQuery request, CancellationToken cancellationToken)
        {
            var workouts = await _workoutSearcher.FindByUserIdAsync(request.UserId, cancellationToken);
            return workouts.ToList();
        }
    }
}
