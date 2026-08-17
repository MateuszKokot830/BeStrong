using Application.Dto.WorkoutPlan;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.UpdateWorkoutPlan
{
    public class UpdateWorkoutPlanCommandHandler(
        IWorkoutPlanRepository workoutPlanRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateWorkoutPlanCommand, ErrorOr<WorkoutPlanDto>>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository = workoutPlanRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<WorkoutPlanDto>> Handle(UpdateWorkoutPlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _workoutPlanRepository.GetByIdAsync(request.PlanId, cancellationToken);
            if (plan is null)
                return Errors.WorkoutPlan.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(plan.CreatedById))
                return Errors.WorkoutPlan.Forbidden;

            if (plan.UsedBy.Count > 0)
                return Errors.WorkoutPlan.InUse;

            plan.Name = request.WorkoutPlanDto.Name;
            plan.Description = request.WorkoutPlanDto.Description;
            plan.Category = request.WorkoutPlanDto.Category;
            plan.IsPublic = request.WorkoutPlanDto.IsPublic;
            plan.WorkoutTemplates.Clear();

            foreach (var templateDto in request.WorkoutPlanDto.WorkoutTemplates)
                plan.WorkoutTemplates.Add(templateDto.ToEntity());

            await _workoutPlanRepository.UpdateAsync(plan, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            var saved = await _workoutPlanRepository.GetByIdAsync(plan.Id, cancellationToken);
            return saved!.ToDto();
        }
    }
}
