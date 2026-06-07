using Application.Dto.Exercise;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.CreateExercise
{
    public record CreateExerciseCommand(ExerciseDto ExerciseDto) : IRequest<ErrorOr<ExerciseDto>>;
}
