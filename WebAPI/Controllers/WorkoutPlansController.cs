using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using ErrorOr;
using Application.Commands.WorkoutPlans.AssignWorkoutPlan;
using Application.Commands.WorkoutPlans.CreateWorkoutPlan;
using Application.Commands.WorkoutPlans.DeleteWorkoutPlan;
using Application.Commands.WorkoutPlans.UnassignWorkoutPlan;
using Application.Dto.WorkoutPlan;
using Application.Queries.WorkoutPlans.GetPublicWorkoutPlans;
using Application.Queries.WorkoutPlans.GetWorkoutPlanById;

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

        [SwaggerOperation(Summary = "Retrieves all public workout plans")]
        [HttpGet]
        public async Task<IActionResult> GetPublicWorkoutPlans()
        {
            ErrorOr<IEnumerable<WorkoutPlanDto>> result = await _mediator.Send(new GetPublicWorkoutPlansQuery());
            return result.Match(
                plans => Ok(plans),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves a workout plan by id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutPlanById(int id)
        {
            ErrorOr<WorkoutPlanDto> result = await _mediator.Send(new GetWorkoutPlanByIdQuery(id));
            return result.Match(
                plan => Ok(plan),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Deletes a workout plan by id")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutPlan(int id)
        {
            ErrorOr<Unit> result = await _mediator.Send(new DeleteWorkoutPlanCommand(id));
            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Assigns a workout plan to the current user")]
        [HttpPost("{id}/assign")]
        public async Task<IActionResult> AssignWorkoutPlan(int id)
        {
            ErrorOr<Unit> result = await _mediator.Send(new AssignWorkoutPlanCommand(id));
            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Unassigns a workout plan from the current user")]
        [HttpDelete("{id}/assign")]
        public async Task<IActionResult> UnassignWorkoutPlan(int id)
        {
            ErrorOr<Unit> result = await _mediator.Send(new UnassignWorkoutPlanCommand(id));
            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }
    }
}
