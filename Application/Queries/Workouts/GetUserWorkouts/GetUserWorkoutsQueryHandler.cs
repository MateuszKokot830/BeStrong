using Application.Dto.Workout;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetUserWorkouts
{
    public class GetUserWorkoutsQueryHandler(
        IWorkoutSearcher workoutSearcher,
        ICurrentUserService currentUserService) : IRequestHandler<GetUserWorkoutsQuery, ErrorOr<IEnumerable<WorkoutDto>>>
    {
        private readonly IWorkoutSearcher _workoutSearcher = workoutSearcher;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<IEnumerable<WorkoutDto>>> Handle(GetUserWorkoutsQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsOwnerOrAdmin(request.UserId))
                return Errors.User.Unauthorized;

            var workouts = await _workoutSearcher.FindByUserIdAsync(request.UserId, cancellationToken);
            return workouts.ToList();
        }
    }
}
