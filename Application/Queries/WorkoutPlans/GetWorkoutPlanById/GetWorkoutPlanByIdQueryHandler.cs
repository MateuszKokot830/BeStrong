using Application.Dto.WorkoutPlan;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.WorkoutPlans.GetWorkoutPlanById
{
    public class GetWorkoutPlanByIdQueryHandler(
        IWorkoutPlanRepository workoutPlanRepository,
        ICurrentUserService currentUserService) : IRequestHandler<GetWorkoutPlanByIdQuery, ErrorOr<WorkoutPlanDto>>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository = workoutPlanRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<WorkoutPlanDto>> Handle(GetWorkoutPlanByIdQuery request, CancellationToken cancellationToken)
        {
            var plan = await _workoutPlanRepository.GetByIdAsync(request.Id, cancellationToken);
            if (plan is null)
                return Errors.WorkoutPlan.NotFound;

            if (!plan.IsPublic && !_currentUserService.IsOwnerOrAdmin(plan.CreatedById))
                return Errors.WorkoutPlan.Unauthorized;

            return plan.ToDto();
        }
    }
}
