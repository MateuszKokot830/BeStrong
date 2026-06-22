using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using ErrorOr;
using Application.Commands.WorkoutPlans.CreateWorkoutPlan;
using Application.Dto.WorkoutPlan;
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

        [SwaggerOperation(Summary = "Retrieves a workout plan by id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutPlanById(int id)
        {
            ErrorOr<WorkoutPlanDto> result = await _mediator.Send(new GetWorkoutPlanByIdQuery(id));
            return result.Match(
                plan => Ok(plan),
                errors => Problem(errors));
        }
    }
}