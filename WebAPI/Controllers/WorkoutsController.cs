using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using Application.Queries.Workouts.GetUserWorkouts;
using Application.Commands.Workouts.CreateWorkout;
using Application.Queries.Workouts.GetExercises;
using Application.Commands.Workouts.CreateExercise;
using Application.Queries.Workouts.GetWorkoutStatistics;
using Application.Interfaces.Services;
using Application.Dto.Workout;
using Application.Dto.Exercise;

namespace WebAPI.Controllers
{
    public class WorkoutsController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ICalculatorService _calculator;

        public WorkoutsController(IMediator mediator, ICalculatorService calculator)
        {
            _mediator = mediator;
            _calculator = calculator;
        }

        [SwaggerOperation(Summary = "Creates a new workout")]
        [HttpPost]
        public async Task<ActionResult> CreateWorkout(WorkoutDto workoutDto)
        {
            await _mediator.Send(new CreateWorkoutCommand(workoutDto));
            return NoContent();
        }

        [SwaggerOperation(Summary = "Retrieves all workouts from specific user")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserWorkouts(int id)
        {
            var workouts = await _mediator.Send(new GetUserWorkoutsQuery(id));
            return Ok(workouts.ToList());
        }

        [SwaggerOperation(Summary = "Retrieves workout statistics from specific user")]
        [HttpGet("statistics/{id}")]
        public async Task<ActionResult> GetWorkoutStatistics(int id)
        {
            var statistics = await _mediator.Send(new GetWorkoutStatisticsQuery(id));
            return Ok(statistics);
        }
        
        [SwaggerOperation(Summary = "Calculate One Rep Max value")]
        [HttpGet("weight/{weight}/reps/{reps}")]
        public async Task<ActionResult> CalculateOneRepMax(int weight, int reps)
        {
            var value = _calculator.CalculateOneRepMax(weight, reps);
            return Ok(value);
        }

        [SwaggerOperation(Summary = "Creates a new exercise")]
        [HttpPost("exercises")]
        public async Task<ActionResult> CreateExercise(ExerciseDto exerciseDto)
        {
            await _mediator.Send(new CreateExerciseCommand(exerciseDto));
            return NoContent();
        }

        [SwaggerOperation(Summary = "Retrieves all exercises")]
        [HttpGet("exercises")]
        public async Task<ActionResult> GetExercises()
        {
            var exercises = await _mediator.Send(new GetExercisesQuery());
            return Ok(exercises.ToList());
        }
    }
}