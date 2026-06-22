using Application.Dto.WorkoutPlan;
using ErrorOr;
using MediatR;

namespace Application.Queries.WorkoutPlans.GetWorkoutPlanById
{
    public record GetWorkoutPlanByIdQuery(int Id) : IRequest<ErrorOr<WorkoutPlanDto>>;
}
