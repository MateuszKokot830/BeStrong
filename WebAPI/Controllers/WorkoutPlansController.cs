using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using Application.Commands.WorkoutPlans.CreateWorkoutPlan;
using Application.Dto.WorkoutPlan;

namespace WebAPI.Controllers
{
    public class WorkoutPlansController : BaseApiController
    {
        private readonly IMediator _mediator;

        public WorkoutPlansController(IMediator mediator) => _mediator = mediator;

        [SwaggerOperation(Summary = "Creates a new workout plan")]
        [HttpPost]
        public async Task<ActionResult> CreateWorkout(WorkoutPlanCreateDto workoutPlanCreateDto)
        {
            await _mediator.Send(new CreateWorkoutPlanCommand(workoutPlanCreateDto));
            return NoContent();
        }
    }
}