using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.DeleteWorkout
{
    public record DeleteWorkoutCommand(int WorkoutId) : IRequest<ErrorOr<Unit>>;
}
