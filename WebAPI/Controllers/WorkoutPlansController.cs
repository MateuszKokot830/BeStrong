using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using ErrorOr;
using Application.Commands.WorkoutPlans.AssignWorkoutPlan;
using Application.Commands.WorkoutPlans.CreateWorkoutPlan;
using Application.Commands.WorkoutPlans.DeleteWorkoutPlan;
using Application.Commands.WorkoutPlans.UnassignWorkoutPlan;
using Application.Commands.WorkoutPlans.UpdateWorkoutPlan;
using Application.Dto.WorkoutPlan;
using Application.Helpers;
using Application.Helpers.Criteria;
using Application.Queries.WorkoutPlans.GetWorkoutPlanById;
using Application.Queries.WorkoutPlans.GetWorkoutPlans;
using Domain.Common;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    public class WorkoutPlansController(IMediator mediator) : BaseApiController
    {
        private readonly IMediator _mediator = mediator;

        [SwaggerOperation(Summary = "Retrieves the available workout plan categories")]
        [HttpGet("categories")]
        public IActionResult GetWorkoutPlanCategories()
        {
            var categories = Enum.GetValues<WorkoutPlanCategory>()
                .Select(category => new WorkoutPlanCategoryDto((int)category, category.ToString(), category.ToDisplayName()))
                .ToList();

            return Ok(categories);
        }

        [SwaggerOperation(Summary = "Creates a new workout plan")]
        [HttpPost]
        public async Task<IActionResult> CreateWorkoutPlan(WorkoutPlanCreateDto workoutPlanCreateDto)
        {
            ErrorOr<WorkoutPlanDto> result = await _mediator.Send(new CreateWorkoutPlanCommand(workoutPlanCreateDto));
            return result.Match(
                plan => Ok(plan),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves workout plans visible to the current user (public or own), filtered and paginated")]
        [HttpGet]
        public async Task<IActionResult> GetWorkoutPlans([FromQuery] WorkoutPlanSearchCriteria criteria)
        {
            ErrorOr<PaginationList<WorkoutPlanDto>> result = await _mediator.Send(new GetWorkoutPlansQuery(criteria));

            if (result.IsError)
                return Problem(result.Errors);

            Response.AddPaginationHeader(new PaginationHeader(
                result.Value.CurrentPage,
                result.Value.PageSize,
                result.Value.TotalItems,
                result.Value.TotalPages));

            return Ok(result.Value);
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

        [SwaggerOperation(Summary = "Updates a workout plan by id")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkoutPlan(int id, WorkoutPlanCreateDto workoutPlanCreateDto)
        {
            ErrorOr<WorkoutPlanDto> result = await _mediator.Send(new UpdateWorkoutPlanCommand(id, workoutPlanCreateDto));
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
