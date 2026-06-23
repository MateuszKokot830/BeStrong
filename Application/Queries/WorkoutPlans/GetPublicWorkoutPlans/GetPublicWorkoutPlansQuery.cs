using Application.Dto.WorkoutPlan;
using ErrorOr;
using MediatR;

namespace Application.Queries.WorkoutPlans.GetPublicWorkoutPlans
{
    public record GetPublicWorkoutPlansQuery : IRequest<ErrorOr<IEnumerable<WorkoutPlanDto>>>;
}
