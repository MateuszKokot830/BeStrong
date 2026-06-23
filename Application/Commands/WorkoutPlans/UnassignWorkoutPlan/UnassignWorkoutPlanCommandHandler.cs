using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.UnassignWorkoutPlan
{
    public class UnassignWorkoutPlanCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UnassignWorkoutPlanCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(UnassignWorkoutPlanCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_currentUserService.UserId, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            if (user.WorkoutPlanId == request.PlanId)
                user.WorkoutPlanId = null;

            await _userRepository.UpdateAsync(user, cancellationToken);
            return Unit.Value;
        }
    }
}
