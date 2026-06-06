using Application.Dto.Exercise;
using MediatR;

namespace Application.Queries.Workouts.GetExercises
{
    public record GetExercisesQuery : IRequest<IEnumerable<ExerciseDto>>;
}