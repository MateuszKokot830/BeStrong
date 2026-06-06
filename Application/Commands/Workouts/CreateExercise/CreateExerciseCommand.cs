using Application.Dto.Exercise;
using MediatR;

namespace Application.Commands.Workouts.CreateExercise
{
    public class CreateExerciseCommand : IRequest
    {
        public required ExerciseDto ExerciseDto { get; set; }
    }
}