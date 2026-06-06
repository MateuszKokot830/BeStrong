using Application.Dto.WorkoutPlan;
using MediatR;

namespace Application.Commands.WorkoutPlans.CreateWorkoutPlan
{
    public class CreateWorkoutPlanCommand : IRequest
    {
        public required WorkoutPlanCreateDto WorkoutPlanCreateDto { get; set; }
    }
}