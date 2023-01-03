using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Domain.Aggregates;

namespace Application.Commands.WorkoutPlans.CreateWorkoutPlan
{
    public class CreateWorkoutPlanCommandHandler : IRequestHandler<CreateWorkoutPlanCommand>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;
        private readonly IMapper _mapper;

        public CreateWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository, IMapper mapper)
        {
            _workoutPlanRepository = workoutPlanRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateWorkoutPlanCommand request, CancellationToken cancellationToken)
        {   
            var plan = _mapper.Map<WorkoutPlan>(request.WorkoutPlanDto);
            await _workoutPlanRepository.AddAsync(plan);

            return Unit.Value;
        }
    }
}