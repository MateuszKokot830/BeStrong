using Application.Dto.WorkoutPlan;
using Application.Helpers;
using Application.Helpers.Criteria;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Queries.WorkoutPlans.GetWorkoutPlans
{
    public class GetWorkoutPlansQueryHandler(
        IWorkoutPlanSearcher workoutPlanSearcher,
        IUserSearcher userSearcher,
        ICurrentUserService currentUserService) : IRequestHandler<GetWorkoutPlansQuery, ErrorOr<PaginationList<WorkoutPlanDto>>>
    {
        private readonly IWorkoutPlanSearcher _workoutPlanSearcher = workoutPlanSearcher;
        private readonly IUserSearcher _userSearcher = userSearcher;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<PaginationList<WorkoutPlanDto>>> Handle(GetWorkoutPlansQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<int>? followedUserIds = request.Criteria.CreatedBy == CreatedByFilter.OnlyFollowers
                ? await _userSearcher.GetFollowedUserIdsAsync(_currentUserService.UserId, cancellationToken)
                : null;

            return await _workoutPlanSearcher.GetPagedAsync(request.Criteria, _currentUserService.UserId, followedUserIds, cancellationToken);
        }
    }
}
