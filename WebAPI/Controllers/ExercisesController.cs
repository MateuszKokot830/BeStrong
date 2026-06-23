using Application.Commands.Workouts.CreateExercise;
using Application.Commands.Workouts.DeleteExercise;
using Application.Commands.Workouts.UpdateExercise;
using Application.Dto.Exercise;
using Application.Queries.Workouts.GetExercises;
using Domain.Common;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    public class ExercisesController(IMediator mediator) : BaseApiController
    {
        private readonly IMediator _mediator = mediator;

        [SwaggerOperation(Summary = "Creates a new exercise")]
        [Authorize(Roles = Roles.Admin)]
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

        [SwaggerOperation(Summary = "Updates an exercise by id")]
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExercise(int id, CreateExerciseDto exerciseDto)
        {
            ErrorOr<ExerciseDto> result = await _mediator.Send(new UpdateExerciseCommand(id, exerciseDto));

            return result.Match(
                exercise => Ok(exercise),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Deletes an exercise by id")]
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            ErrorOr<Unit> result = await _mediator.Send(new DeleteExerciseCommand(id));

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }
    }
}
