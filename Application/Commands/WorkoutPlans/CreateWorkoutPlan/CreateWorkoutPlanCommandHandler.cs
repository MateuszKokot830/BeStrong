using Application.Dto.WorkoutPlan;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.CreateWorkoutPlan
{
    public class CreateWorkoutPlanCommandHandler(
        IWorkoutPlanRepository workoutPlanRepository,
        ICurrentUserService currentUserService) : IRequestHandler<CreateWorkoutPlanCommand, ErrorOr<WorkoutPlanDto>>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository = workoutPlanRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<WorkoutPlanDto>> Handle(CreateWorkoutPlanCommand request, CancellationToken cancellationToken)
        {
            var plan = request.WorkoutPlanCreateDto.ToEntity(_currentUserService.UserId);
            await _workoutPlanRepository.AddAsync(plan, cancellationToken);
            return plan.ToDto();
        }
    }
}
