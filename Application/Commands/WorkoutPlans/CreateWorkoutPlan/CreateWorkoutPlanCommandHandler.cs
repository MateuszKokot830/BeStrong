using AutoMapper;
using MediatR;
using Domain.Aggregates;
using Application.Interfaces.Repositories;

namespace Application.Commands.WorkoutPlans.CreateWorkoutPlan
{
    public class CreateWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository, IMapper mapper) : IRequestHandler<CreateWorkoutPlanCommand>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository = workoutPlanRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(CreateWorkoutPlanCommand request, CancellationToken cancellationToken)
        {
            var plan = _mapper.Map<WorkoutPlan>(request.WorkoutPlanCreateDto);
            await _workoutPlanRepository.AddAsync(plan);

            return Unit.Value;
        }
    }
}