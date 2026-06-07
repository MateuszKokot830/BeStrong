using Application.Dto.Exercise;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetExercises
{
    public record GetExercisesQuery : IRequest<ErrorOr<IEnumerable<ExerciseDto>>>;
}
