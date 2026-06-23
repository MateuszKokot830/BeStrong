using Application.Dto.Workout;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.UpdateWorkout
{
    public class UpdateWorkoutCommandHandler(
        IWorkoutRepository workoutRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UpdateWorkoutCommand, ErrorOr<WorkoutDto>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<WorkoutDto>> Handle(UpdateWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workout = await _workoutRepository.GetByIdAsync(request.WorkoutId, cancellationToken);
            if (workout is null)
                return Errors.Workout.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(workout.UserId ?? 0))
                return Errors.Workout.Unauthorized;

            workout.Name = request.WorkoutDto.Name;
            workout.WorkoutExercises.Clear();
            foreach (var exerciseDto in request.WorkoutDto.Exercises)
                workout.WorkoutExercises.Add(exerciseDto.ToEntity());

            await _workoutRepository.UpdateAsync(workout, cancellationToken);
            return workout.ToDto();
        }
    }
}
