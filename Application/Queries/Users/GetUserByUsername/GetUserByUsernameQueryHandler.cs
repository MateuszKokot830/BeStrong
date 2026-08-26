using Application.Dto.User;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Domain.Errors;
using Domain.Services;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUserByUsername
{
    public class GetUserByUsernameQueryHandler(
        IUserSearcher userSearcher,
        ICurrentUserService currentUserService) : IRequestHandler<GetUserByUsernameQuery, ErrorOr<UserDto>>
    {
        private readonly IUserSearcher _userSearcher = userSearcher;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<UserDto>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userSearcher.FindByUsernameAsync(request.Username, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            var isOwnerOrAdmin = _currentUserService.IsOwnerOrAdmin(user.Id);
            if (isOwnerOrAdmin)
                return user;

            var settings = await _userSearcher.GetSettingsAsync(user.Id, cancellationToken);
            var followedIds = await _userSearcher.GetFollowedUserIdsAsync(_currentUserService.UserId, cancellationToken);
            var isFollower = followedIds.Contains(user.Id);

            var canViewPhotos = ProfileVisibilityEvaluator.CanView(settings.PhotosVisibility, isOwnerOrAdmin, isFollower);
            var canViewWorkouts = ProfileVisibilityEvaluator.CanView(settings.WorkoutsVisibility, isOwnerOrAdmin, isFollower);
            var canViewWorkoutPlan = ProfileVisibilityEvaluator.CanView(settings.WorkoutPlanVisibility, isOwnerOrAdmin, isFollower);
            var canViewMeasurements = ProfileVisibilityEvaluator.CanView(settings.MeasurementsVisibility, isOwnerOrAdmin, isFollower);

            return user with
            {
                Photos = canViewPhotos ? user.Photos : [],
                WorkoutPlanId = canViewWorkoutPlan ? user.WorkoutPlanId : null,
                WorkoutPlanName = canViewWorkoutPlan ? user.WorkoutPlanName : null,
                Measurements = canViewMeasurements ? user.Measurements : null,
                CanViewPhotos = canViewPhotos,
                CanViewWorkouts = canViewWorkouts,
                CanViewWorkoutPlan = canViewWorkoutPlan,
                CanViewMeasurements = canViewMeasurements
            };
        }
    }
}
