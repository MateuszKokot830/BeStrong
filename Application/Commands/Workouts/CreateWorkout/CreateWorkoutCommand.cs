using Application.Dto.Workout;
using MediatR;

namespace Application.Commands.Workouts.CreateWorkout
{
    public class CreateWorkoutCommand : IRequest
    {
        public required WorkoutDto WorkoutDto { get; set; }
    }
}