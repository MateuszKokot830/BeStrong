using Application.Dto.WorkoutPlan;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Aggregates;
using ErrorOr;
using MediatR;

namespace Application.Commands.WorkoutPlans.CreateWorkoutPlan
{
    public class CreateWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository, IMapper mapper)
        : IRequestHandler<CreateWorkoutPlanCommand, ErrorOr<WorkoutPlanDto>>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository = workoutPlanRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<WorkoutPlanDto>> Handle(CreateWorkoutPlanCommand request, CancellationToken cancellationToken)
        {
            var plan = _mapper.Map<WorkoutPlan>(request.WorkoutPlanCreateDto);
            await _workoutPlanRepository.AddAsync(plan, cancellationToken);
            return _mapper.Map<WorkoutPlanDto>(plan);
        }
    }
}
