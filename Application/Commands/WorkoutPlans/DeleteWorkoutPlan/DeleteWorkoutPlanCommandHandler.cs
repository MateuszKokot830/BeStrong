using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.DeleteWorkoutPlan
{
    public class DeleteWorkoutPlanCommandHandler(
        IWorkoutPlanRepository workoutPlanRepository,
        ICurrentUserService currentUserService) : IRequestHandler<DeleteWorkoutPlanCommand, ErrorOr<Unit>>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository = workoutPlanRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(DeleteWorkoutPlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _workoutPlanRepository.GetByIdAsync(request.PlanId, cancellationToken);
            if (plan is null)
                return Errors.WorkoutPlan.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(plan.CreatedById))
                return Errors.WorkoutPlan.Unauthorized;

            if (plan.UsedBy.Count > 0)
                return Errors.WorkoutPlan.InUse;

            await _workoutPlanRepository.DeleteAsync(plan, cancellationToken);
            return Unit.Value;
        }
    }
}
