using Application.Dto;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.CreateWorkout
{
    public class CreateWorkoutCommand : IRequest
    {
        public WorkoutDto WorkoutDto { get; set; }
    }
}