using Application.Dto.WorkoutPlan;
using Application.Interfaces.Searchers;
using ErrorOr;
using MediatR;

namespace Application.Queries.WorkoutPlans.GetPublicWorkoutPlans
{
    public class GetPublicWorkoutPlansQueryHandler(IWorkoutPlanSearcher workoutPlanSearcher)
        : IRequestHandler<GetPublicWorkoutPlansQuery, ErrorOr<IEnumerable<WorkoutPlanDto>>>
    {
        private readonly IWorkoutPlanSearcher _workoutPlanSearcher = workoutPlanSearcher;

        public async Task<ErrorOr<IEnumerable<WorkoutPlanDto>>> Handle(GetPublicWorkoutPlansQuery request, CancellationToken cancellationToken)
        {
            var plans = await _workoutPlanSearcher.GetPublicAsync(cancellationToken);
            return plans.ToList();
        }
    }
}
