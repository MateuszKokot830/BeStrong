using Application.Dto.Workout;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetUserWorkouts
{
    public record GetUserWorkoutsQuery(int UserId) : IRequest<ErrorOr<IEnumerable<WorkoutDto>>>;
}
