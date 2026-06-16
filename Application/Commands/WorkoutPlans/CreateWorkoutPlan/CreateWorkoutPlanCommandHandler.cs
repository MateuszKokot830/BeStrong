using Application.Dto.WorkoutPlan;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.CreateWorkoutPlan
{
    public class CreateWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository)
        : IRequestHandler<CreateWorkoutPlanCommand, ErrorOr<WorkoutPlanDto>>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository = workoutPlanRepository;

        public async Task<ErrorOr<WorkoutPlanDto>> Handle(CreateWorkoutPlanCommand request, CancellationToken cancellationToken)
        {
            var plan = request.WorkoutPlanCreateDto.ToEntity();
            await _workoutPlanRepository.AddAsync(plan, cancellationToken);
            return plan.ToDto();
        }
    }
}
