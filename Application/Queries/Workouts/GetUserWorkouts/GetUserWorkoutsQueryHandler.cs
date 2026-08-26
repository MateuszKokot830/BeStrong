using Application.Dto.Workout;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Domain.Services;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetUserWorkouts
{
    public class GetUserWorkoutsQueryHandler(
        IWorkoutSearcher workoutSearcher,
        IUserSearcher userSearcher,
        ICurrentUserService currentUserService) : IRequestHandler<GetUserWorkoutsQuery, ErrorOr<IEnumerable<WorkoutDto>>>
    {
        private readonly IWorkoutSearcher _workoutSearcher = workoutSearcher;
        private readonly IUserSearcher _userSearcher = userSearcher;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<IEnumerable<WorkoutDto>>> Handle(GetUserWorkoutsQuery request, CancellationToken cancellationToken)
        {
            var isOwnerOrAdmin = _currentUserService.IsOwnerOrAdmin(request.UserId);
            if (!isOwnerOrAdmin)
            {
                var settings = await _userSearcher.GetSettingsAsync(request.UserId, cancellationToken);
                var followedIds = await _userSearcher.GetFollowedUserIdsAsync(_currentUserService.UserId, cancellationToken);
                var isFollower = followedIds.Contains(request.UserId);

                if (!ProfileVisibilityEvaluator.CanView(settings.WorkoutsVisibility, isOwnerOrAdmin, isFollower))
                    return new List<WorkoutDto>();
            }

            var workouts = await _workoutSearcher.FindByUserIdAsync(request.UserId, cancellationToken);
            return workouts.ToList();
        }
    }
}
