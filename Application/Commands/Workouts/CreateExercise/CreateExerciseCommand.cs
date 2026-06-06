using Application.Dto.Exercise;
using MediatR;

namespace Application.Commands.Workouts.CreateExercise
{
    public record CreateExerciseCommand(ExerciseDto ExerciseDto) : IRequest;
}