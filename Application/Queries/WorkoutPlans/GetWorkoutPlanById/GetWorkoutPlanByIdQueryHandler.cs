using Application.Dto.WorkoutPlan;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.WorkoutPlans.GetWorkoutPlanById
{
    public class GetWorkoutPlanByIdQueryHandler(
        IWorkoutPlanSearcher workoutPlanSearcher,
        ICurrentUserService currentUserService) : IRequestHandler<GetWorkoutPlanByIdQuery, ErrorOr<WorkoutPlanDto>>
    {
        private readonly IWorkoutPlanSearcher _workoutPlanSearcher = workoutPlanSearcher;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<WorkoutPlanDto>> Handle(GetWorkoutPlanByIdQuery request, CancellationToken cancellationToken)
        {
            var plan = await _workoutPlanSearcher.FindByIdAsync(request.Id, cancellationToken);
            if (plan is null)
                return Errors.WorkoutPlan.NotFound;

            if (!plan.IsPublic && !_currentUserService.IsOwnerOrAdmin(plan.CreatedById))
                return Errors.WorkoutPlan.Forbidden;

            return plan;
        }
    }
}
