using Application.Dto.Workout;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Errors;
using Domain.Factories;
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

            var exercises = request.WorkoutDto.WorkoutExercises
                .Select(we => we.ToEntity())
                .ToList();

            var workout = WorkoutFactory.Create(
                request.WorkoutDto.UserId ?? _currentUserService.UserId,
                request.WorkoutDto.Name,
                exercises);

            await _workoutRepository.AddAsync(workout, cancellationToken);
            return workout.ToDto();
        }
    }
}
