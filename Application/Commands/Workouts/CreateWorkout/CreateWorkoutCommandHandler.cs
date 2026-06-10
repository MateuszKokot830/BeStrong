using Application.Dto.Workout;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Aggregates;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.CreateWorkout
{
    public class CreateWorkoutCommandHandler(IWorkoutRepository workoutRepository, IMapper mapper)
        : IRequestHandler<CreateWorkoutCommand, ErrorOr<WorkoutDto>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<WorkoutDto>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workout = _mapper.Map<Workout>(request.WorkoutDto);
            await _workoutRepository.AddAsync(workout, cancellationToken);
            return _mapper.Map<WorkoutDto>(workout);
        }
    }
}
