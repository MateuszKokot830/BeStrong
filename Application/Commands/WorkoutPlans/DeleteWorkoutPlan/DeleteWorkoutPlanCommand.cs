using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.DeleteWorkoutPlan
{
    public record DeleteWorkoutPlanCommand(int PlanId) : IRequest<ErrorOr<Unit>>;
}
