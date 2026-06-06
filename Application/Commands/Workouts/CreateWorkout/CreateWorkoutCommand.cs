using Application.Dto.Workout;
using MediatR;

namespace Application.Commands.Workouts.CreateWorkout
{
    public record CreateWorkoutCommand(WorkoutDto WorkoutDto) : IRequest;
}