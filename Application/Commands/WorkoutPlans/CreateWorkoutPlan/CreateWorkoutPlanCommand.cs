using Application.Dto.WorkoutPlan;
using MediatR;

namespace Application.Commands.WorkoutPlans.CreateWorkoutPlan
{
    public record CreateWorkoutPlanCommand(WorkoutPlanCreateDto WorkoutPlanCreateDto) : IRequest;
}