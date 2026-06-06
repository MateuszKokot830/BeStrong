using AutoMapper;
using MediatR;
using Domain.Aggregates;
using Application.Interfaces.Repositories;

namespace Application.Commands.Workouts.CreateWorkout
{
    public class CreateWorkoutCommandHandler(IWorkoutRepository workoutRepository, IMapper mapper) : IRequestHandler<CreateWorkoutCommand>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workout = _mapper.Map<Workout>(request.WorkoutDto);
            await _workoutRepository.AddAsync(workout);

            return Unit.Value;
        }
    }
}