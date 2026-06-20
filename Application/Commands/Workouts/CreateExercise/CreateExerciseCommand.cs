using Application.Dto.Exercise;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.CreateExercise
{
    public record CreateExerciseCommand(CreateExerciseDto ExerciseDto) : IRequest<ErrorOr<ExerciseDto>>;
}
