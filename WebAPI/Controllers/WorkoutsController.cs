using Application.Commands.Workouts.CreateWorkout;
using Application.Commands.Workouts.DeleteWorkout;
using Application.Commands.Workouts.UpdateWorkout;
using Application.Dto.Workout;
using Application.Dto.Statistics;
using Application.Queries.Workouts.GetUserWorkouts;
using Application.Queries.Workouts.GetOneRepMax;
using Application.Queries.Workouts.GetWorkoutStatistics;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    public class WorkoutsController(IMediator mediator) : BaseApiController
    {
        private readonly IMediator _mediator = mediator;

        [SwaggerOperation(Summary = "Creates a new workout")]
        [HttpPost]
        public async Task<IActionResult> CreateWorkout(CreateWorkoutDto workoutDto)
        {
            ErrorOr<WorkoutDto> result = await _mediator.Send(new CreateWorkoutCommand(workoutDto));

            return result.Match(
                workout => Ok(workout),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves all workouts from specific user")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserWorkouts(int id)
        {
            ErrorOr<IEnumerable<WorkoutDto>> result = await _mediator.Send(new GetUserWorkoutsQuery(id));

            return result.Match(
                workouts => Ok(workouts),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves workout statistics from specific user")]
        [HttpGet("statistics/{id}")]
        public async Task<IActionResult> GetWorkoutStatistics(int id)
        {
            ErrorOr<StatisticsDto> result = await _mediator.Send(new GetWorkoutStatisticsQuery(id));

            return result.Match(
                statistics => Ok(statistics),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Calculate One Rep Max value")]
        [HttpGet("weight/{weight}/reps/{reps}")]
        public async Task<IActionResult> CalculateOneRepMax(int weight, int reps)
        {
            ErrorOr<int> result = await _mediator.Send(new GetOneRepMaxQuery(weight, reps));

            return result.Match(
                value => Ok(value),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Updates a workout by id")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkout(int id, CreateWorkoutDto workoutDto)
        {
            ErrorOr<WorkoutDto> result = await _mediator.Send(new UpdateWorkoutCommand(id, workoutDto));

            return result.Match(
                workout => Ok(workout),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Deletes a workout by id")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            ErrorOr<Unit> result = await _mediator.Send(new DeleteWorkoutCommand(id));

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }
    }
}
