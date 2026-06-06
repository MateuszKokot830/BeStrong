using Application.Dto.Exercise;
using MediatR;

namespace Application.Queries.Workouts.GetExercises
{
    public class GetExercisesQuery : IRequest<IEnumerable<ExerciseDto>>
    {
    }
}