using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Domain.Aggregates;

namespace Application.Commands.Workouts.CreateWorkout
{
    public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IMapper _mapper;

        public CreateWorkoutCommandHandler(IWorkoutRepository workoutRepository, IMapper mapper)
        {
            _workoutRepository = workoutRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {   
            var workout = _mapper.Map<Workout>(request.WorkoutDto);
            await _workoutRepository.AddAsync(workout);

            return Unit.Value;
        }
    }
}