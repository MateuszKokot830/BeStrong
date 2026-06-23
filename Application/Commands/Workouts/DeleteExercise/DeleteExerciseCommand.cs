using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.DeleteExercise
{
    public record DeleteExerciseCommand(int ExerciseId) : IRequest<ErrorOr<Unit>>;
}
