using Application.Dto.Workout;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.UpdateWorkout
{
    public record UpdateWorkoutCommand(int WorkoutId, CreateWorkoutDto WorkoutDto) : IRequest<ErrorOr<WorkoutDto>>;
}
