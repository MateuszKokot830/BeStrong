using Application.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using Application.Commands.WorkoutPlans.CreateWorkoutPlan;

namespace WebAPI.Controllers
{
    public class WorkoutPlansController : BaseApiController
    {
        private readonly IMediator _mediator;

        public WorkoutPlansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Creates a new workout plan")]
        [HttpPost]
        public async Task<ActionResult> CreateWorkout(WorkoutPlanDto workoutPlanDto)
        {
            await _mediator.Send(new CreateWorkoutPlanCommand() {WorkoutPlanDto = workoutPlanDto});
            return NoContent();
        }
    }
}