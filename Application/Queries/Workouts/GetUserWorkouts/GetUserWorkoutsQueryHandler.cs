using Application.Dto.Workout;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetUserWorkouts
{
    public class GetUserWorkoutsQueryHandler(IWorkoutRepository workoutRepository)
        : IRequestHandler<GetUserWorkoutsQuery, ErrorOr<IEnumerable<WorkoutDto>>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;

        public async Task<ErrorOr<IEnumerable<WorkoutDto>>> Handle(GetUserWorkoutsQuery request, CancellationToken cancellationToken)
        {
            var workouts = await _workoutRepository.GetUserWorkoutsAsync(request.UserId, cancellationToken);
            return workouts.Select(w => w.ToDto()).ToList();
        }
    }
}
