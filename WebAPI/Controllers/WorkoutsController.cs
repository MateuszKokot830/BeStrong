using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using ErrorOr;
using Application.Dto.Exercise;
using Application.Dto.Statistics;
using Application.Dto.Workout;
using Application.Commands.Workouts.CreateExercise;
using Application.Commands.Workouts.CreateWorkout;
using Application.Interfaces.Services;
using Application.Queries.Workouts.GetExercises;
using Application.Queries.Workouts.GetUserWorkouts;
using Application.Queries.Workouts.GetWorkoutStatistics;

namespace WebAPI.Controllers
{
    public class WorkoutsController(IMediator mediator, ICalculatorService calculator) : BaseApiController
    {
        private readonly IMediator _mediator = mediator;
        private readonly ICalculatorService _calculator = calculator;

        [SwaggerOperation(Summary = "Creates a new workout")]
        [HttpPost]
        public async Task<IActionResult> CreateWorkout(WorkoutDto workoutDto)
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
        public IActionResult CalculateOneRepMax(int weight, int reps)
        {
            var value = _calculator.CalculateOneRepMax(weight, reps);
            
            return Ok(value);
        }

        [SwaggerOperation(Summary = "Creates a new exercise")]
        [HttpPost("exercises")]
        public async Task<IActionResult> CreateExercise(ExerciseDto exerciseDto)
        {
            ErrorOr<ExerciseDto> result = await _mediator.Send(new CreateExerciseCommand(exerciseDto));
            
            return result.Match(
                exercise => Ok(exercise),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves all exercises")]
        [HttpGet("exercises")]
        public async Task<IActionResult> GetExercises()
        {
            ErrorOr<IEnumerable<ExerciseDto>> result = await _mediator.Send(new GetExercisesQuery());
            
            return result.Match(
                exercises => Ok(exercises),
                errors => Problem(errors));
        }
    }
}