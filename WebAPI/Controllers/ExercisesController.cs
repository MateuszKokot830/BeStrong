using Application.Commands.Workouts.CreateExercise;
using Application.Dto.Exercise;
using Application.Queries.Workouts.GetExercises;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    public class ExercisesController(IMediator mediator) : BaseApiController
    {
        private readonly IMediator _mediator = mediator;

        [SwaggerOperation(Summary = "Creates a new exercise")]
        [HttpPost]
        public async Task<IActionResult> CreateExercise(CreateExerciseDto exerciseDto)
        {
            ErrorOr<ExerciseDto> result = await _mediator.Send(new CreateExerciseCommand(exerciseDto));

            return result.Match(
                exercise => Ok(exercise),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves all exercises")]
        [HttpGet]
        public async Task<IActionResult> GetExercises()
        {
            ErrorOr<IEnumerable<ExerciseDto>> result = await _mediator.Send(new GetExercisesQuery());

            return result.Match(
                exercises => Ok(exercises),
                errors => Problem(errors));
        }
    }
}
