using AutoMapper;
using MediatR;
using Domain.Aggregates;
using Domain.Errors;
using ErrorOr;
using Application.Dto.Workout;
using Application.Interfaces.Repositories;

namespace Application.Commands.Workouts.CreateWorkout
{
    public class CreateWorkoutCommandHandler(IWorkoutRepository workoutRepository, IMapper mapper) : IRequestHandler<CreateWorkoutCommand, ErrorOr<WorkoutDto>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<WorkoutDto>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var workout = _mapper.Map<Workout>(request.WorkoutDto);
                await _workoutRepository.AddAsync(workout, cancellationToken);
                return _mapper.Map<WorkoutDto>(workout);
            }
            catch (Exception)
            {
                return Errors.Workout.CreationFailed;
            }
        }
    }
}
