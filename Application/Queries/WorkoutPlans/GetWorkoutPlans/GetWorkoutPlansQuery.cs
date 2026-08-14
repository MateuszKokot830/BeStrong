using Application.Dto.WorkoutPlan;
using Application.Helpers;
using Application.Helpers.Criteria;
using ErrorOr;
using MediatR;

namespace Application.Queries.WorkoutPlans.GetWorkoutPlans
{
    public record GetWorkoutPlansQuery(WorkoutPlanSearchCriteria Criteria) : IRequest<ErrorOr<PaginationList<WorkoutPlanDto>>>;
}
