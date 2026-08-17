using Application.Dto.Workout;
using Application.Helpers;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetWorkouts
{
    public class GetWorkoutsQueryHandler(
        IWorkoutSearcher workoutSearcher,
        ICurrentUserService currentUserService) : IRequestHandler<GetWorkoutsQuery, ErrorOr<PaginationList<WorkoutDto>>>
    {
        private readonly IWorkoutSearcher _workoutSearcher = workoutSearcher;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<PaginationList<WorkoutDto>>> Handle(GetWorkoutsQuery request, CancellationToken cancellationToken)
        {
            return await _workoutSearcher.GetPagedAsync(request.Criteria, _currentUserService.UserId, cancellationToken);
        }
    }
}
