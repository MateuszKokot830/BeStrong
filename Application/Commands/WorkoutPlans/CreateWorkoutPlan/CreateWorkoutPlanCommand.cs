using Application.Dto;
using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.CreateWorkoutPlan
{
    public class CreateWorkoutPlanCommand : IRequest
    {
        public WorkoutPlanDto WorkoutPlanDto { get; set; }
    }
}