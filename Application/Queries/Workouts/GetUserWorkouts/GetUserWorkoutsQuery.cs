using Application.Dto.Workout;
using MediatR;

namespace Application.Queries.Workouts.GetUserWorkouts
{
    public class GetUserWorkoutsQuery : IRequest<IEnumerable<WorkoutDto>>
    {
        public int UserId { get; set; }
    }
}