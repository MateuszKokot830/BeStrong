using Application.Dto.Workout;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.CreateWorkout
{
    public class CreateWorkoutCommandHandler(
        IWorkoutRepository workoutRepository,
        ICurrentUserService currentUserService) : IRequestHandler<CreateWorkoutCommand, ErrorOr<WorkoutDto>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<WorkoutDto>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {
            if (request.WorkoutDto.UserId.HasValue &&
                !_currentUserService.IsOwnerOrAdmin(request.WorkoutDto.UserId.Value))
                return Errors.User.Unauthorized;

            var workout = request.WorkoutDto.ToEntity();
            await _workoutRepository.AddAsync(workout, cancellationToken);
            return workout.ToDto();
        }
    }
}
