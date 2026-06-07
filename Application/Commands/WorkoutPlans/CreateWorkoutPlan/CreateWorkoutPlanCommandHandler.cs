using AutoMapper;
using MediatR;
using Domain.Aggregates;
using Domain.Errors;
using ErrorOr;
using Application.Dto.WorkoutPlan;
using Application.Interfaces.Repositories;

namespace Application.Commands.WorkoutPlans.CreateWorkoutPlan
{
    public class CreateWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository, IMapper mapper) : IRequestHandler<CreateWorkoutPlanCommand, ErrorOr<WorkoutPlanDto>>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository = workoutPlanRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<WorkoutPlanDto>> Handle(CreateWorkoutPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var plan = _mapper.Map<WorkoutPlan>(request.WorkoutPlanCreateDto);
                await _workoutPlanRepository.AddAsync(plan, cancellationToken);
                return _mapper.Map<WorkoutPlanDto>(plan);
            }
            catch (Exception)
            {
                return Errors.WorkoutPlan.CreationFailed;
            }
        }
    }
}
