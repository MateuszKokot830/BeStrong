using Application.Dto.Workout;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.CreateWorkout
{
    public record CreateWorkoutCommand(WorkoutDto WorkoutDto) : IRequest<ErrorOr<WorkoutDto>>;
}
