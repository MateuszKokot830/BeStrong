using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using ErrorOr;
using Application.Commands.WorkoutPlans.CreateWorkoutPlan;
using Application.Dto.WorkoutPlan;

namespace WebAPI.Controllers
{
    public class WorkoutPlansController(IMediator mediator) : BaseApiController
    {
        private readonly IMediator _mediator = mediator;

        [SwaggerOperation(Summary = "Creates a new workout plan")]
        [HttpPost]
        public async Task<IActionResult> CreateWorkoutPlan(WorkoutPlanCreateDto workoutPlanCreateDto)
        {
            ErrorOr<WorkoutPlanDto> result = await _mediator.Send(new CreateWorkoutPlanCommand(workoutPlanCreateDto));
            return result.Match(
                plan => Ok(plan),
                errors => Problem(errors));
        }
    }
}