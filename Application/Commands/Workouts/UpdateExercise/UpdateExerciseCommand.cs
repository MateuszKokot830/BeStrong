using Application.Dto.Exercise;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.UpdateExercise
{
    public record UpdateExerciseCommand(int ExerciseId, CreateExerciseDto ExerciseDto) : IRequest<ErrorOr<ExerciseDto>>;
}
