using Application.Dto.WorkoutPlan;
using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.CreateWorkoutPlan
{
    public record CreateWorkoutPlanCommand(WorkoutPlanCreateDto WorkoutPlanCreateDto) : IRequest<ErrorOr<WorkoutPlanDto>>;
}
