using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.AssignWorkoutPlan
{
    public record AssignWorkoutPlanCommand(int PlanId) : IRequest<ErrorOr<Unit>>;
}
