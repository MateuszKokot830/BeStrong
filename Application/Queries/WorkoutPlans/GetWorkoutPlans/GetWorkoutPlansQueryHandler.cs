using Application.Dto.WorkoutPlan;
using Application.Helpers;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Queries.WorkoutPlans.GetWorkoutPlans
{
    public class GetWorkoutPlansQueryHandler(
        IWorkoutPlanSearcher workoutPlanSearcher,
        ICurrentUserService currentUserService) : IRequestHandler<GetWorkoutPlansQuery, ErrorOr<PaginationList<WorkoutPlanDto>>>
    {
        private readonly IWorkoutPlanSearcher _workoutPlanSearcher = workoutPlanSearcher;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<PaginationList<WorkoutPlanDto>>> Handle(GetWorkoutPlansQuery request, CancellationToken cancellationToken)
        {
            return await _workoutPlanSearcher.GetPagedAsync(request.Criteria, _currentUserService.UserId, cancellationToken);
        }
    }
}
