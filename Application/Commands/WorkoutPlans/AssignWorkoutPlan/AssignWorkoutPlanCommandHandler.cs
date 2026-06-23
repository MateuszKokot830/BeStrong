using Application.Interfaces.Repositories;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.AssignWorkoutPlan
{
    public class AssignWorkoutPlanCommandHandler(
        IWorkoutPlanSearcher workoutPlanSearcher,
        IUserRepository userRepository,
        ICurrentUserService currentUserService) : IRequestHandler<AssignWorkoutPlanCommand, ErrorOr<Unit>>
    {
        private readonly IWorkoutPlanSearcher _workoutPlanSearcher = workoutPlanSearcher;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(AssignWorkoutPlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _workoutPlanSearcher.FindByIdAsync(request.PlanId, cancellationToken);
            if (plan is null)
                return Errors.WorkoutPlan.NotFound;

            if (!plan.IsPublic && !_currentUserService.IsOwnerOrAdmin(plan.CreatedById))
                return Errors.WorkoutPlan.Unauthorized;

            var user = await _userRepository.GetByIdAsync(_currentUserService.UserId, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            user.WorkoutPlanId = plan.Id;
            await _userRepository.UpdateAsync(user, cancellationToken);
            return Unit.Value;
        }
    }
}
