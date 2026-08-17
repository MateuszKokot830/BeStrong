using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.DeleteWorkout
{
    public class DeleteWorkoutCommandHandler(
        IWorkoutRepository workoutRepository,
        ICurrentUserService currentUserService) : IRequestHandler<DeleteWorkoutCommand, ErrorOr<Unit>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(DeleteWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workout = await _workoutRepository.GetByIdAsync(request.WorkoutId, cancellationToken);
            if (workout is null)
                return Errors.Workout.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(workout.UserId ?? 0))
                return Errors.Workout.Forbidden;

            await _workoutRepository.DeleteAsync(workout, cancellationToken);
            return Unit.Value;
        }
    }
}
