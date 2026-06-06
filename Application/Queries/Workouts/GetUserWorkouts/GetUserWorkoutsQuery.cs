using Application.Dto.Workout;
using MediatR;

namespace Application.Queries.Workouts.GetUserWorkouts
{
    public record GetUserWorkoutsQuery(int UserId) : IRequest<IEnumerable<WorkoutDto>>;
}