using Application.Dto;
using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.CreateWorkoutPlan
{
    public class CreateWorkoutPlanCommand : IRequest
    {
        public WorkoutPlanCreateDto WorkoutPlanCreateDto { get; set; }
    }
}