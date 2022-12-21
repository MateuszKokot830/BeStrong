using Application.Dto;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.CreateExercise
{
    public class CreateExerciseCommand : IRequest
    {
        public ExerciseDto ExerciseDto { get; set; }
    }
}