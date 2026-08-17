using Application.Dto.Workout;
using Application.Helpers;
using Application.Helpers.Criteria;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetWorkouts
{
    public record GetWorkoutsQuery(WorkoutSearchCriteria Criteria) : IRequest<ErrorOr<PaginationList<WorkoutDto>>>;
}
