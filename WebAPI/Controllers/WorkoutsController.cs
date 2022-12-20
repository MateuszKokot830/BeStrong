using Application.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using Application.Queries.Workouts.GetUserWorkouts;
using Application.Commands.Workouts.CreateWorkout;

namespace WebAPI.Controllers
{
    public class WorkoutsController : BaseApiController
    {
         private readonly IMediator _mediator;

        public WorkoutsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Creates a new workout")]
        [HttpPost]
        public async Task<ActionResult> CreateWorkout(WorkoutDto workoutDto)
        {
            await _mediator.Send(new CreateWorkoutCommand() {WorkoutDto = workoutDto});
            return NoContent();
        }

        [SwaggerOperation(Summary = "Retrieves all workouts from specific user")]
        [HttpGet]
        public async Task<ActionResult> GetUserWorkouts()
        {
            var posts = await _mediator.Send(new GetUserWorkoutsQuery());
            return Ok(posts.ToList());
        }
    }
}