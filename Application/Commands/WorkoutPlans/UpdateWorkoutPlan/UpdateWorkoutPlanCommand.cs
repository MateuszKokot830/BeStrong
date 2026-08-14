using Application.Dto.WorkoutPlan;
using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.UpdateWorkoutPlan
{
    public record UpdateWorkoutPlanCommand(int PlanId, WorkoutPlanCreateDto WorkoutPlanDto) : IRequest<ErrorOr<WorkoutPlanDto>>;
}
