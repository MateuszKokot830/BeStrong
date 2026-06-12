using Application.Dto.Workout;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Aggregates;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.CreateWorkout
{
    public class CreateWorkoutCommandHandler(
        IWorkoutRepository workoutRepository,
        ICurrentUserService currentUserService,
        IMapper mapper) : IRequestHandler<CreateWorkoutCommand, ErrorOr<WorkoutDto>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<WorkoutDto>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {
            if (request.WorkoutDto.UserId.HasValue &&
                !_currentUserService.IsOwnerOrAdmin(request.WorkoutDto.UserId.Value))
                return Errors.User.Unauthorized;

            var workout = _mapper.Map<Workout>(request.WorkoutDto);
            await _workoutRepository.AddAsync(workout, cancellationToken);
            return _mapper.Map<WorkoutDto>(workout);
        }
    }
}
