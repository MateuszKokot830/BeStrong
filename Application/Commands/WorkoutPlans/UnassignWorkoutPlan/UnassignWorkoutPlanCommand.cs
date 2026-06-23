using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.UnassignWorkoutPlan
{
    public record UnassignWorkoutPlanCommand(int PlanId) : IRequest<ErrorOr<Unit>>;
}
