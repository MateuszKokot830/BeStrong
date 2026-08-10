using Application.Dto.Workout;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Factories;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.CreateWorkout
{
    public class CreateWorkoutCommandHandler(
        IWorkoutRepository workoutRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateWorkoutCommand, ErrorOr<WorkoutDto>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<WorkoutDto>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {
            var exercises = request.WorkoutDto.Exercises
                .Select(we => we.ToEntity())
                .ToList();

            var workout = WorkoutFactory.Create(
                _currentUserService.UserId,
                request.WorkoutDto.Name,
                exercises);

            await _workoutRepository.AddAsync(workout, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return workout.ToDto();
        }
    }
}
