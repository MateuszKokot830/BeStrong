using Application.Commands.Workouts.UpdateWorkout;
using Application.Dto.Workout;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Workouts.UpdateWorkout
{
    public class UpdateWorkoutCommandHandlerTests
    {
        private readonly Mock<IWorkoutRepository> _workoutRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly UpdateWorkoutCommandHandler _sut;

        public UpdateWorkoutCommandHandlerTests()
        {
            _sut = new UpdateWorkoutCommandHandler(_workoutRepository.Object, _currentUserService.Object);
        }

        private static CreateWorkoutDto Dto(string? name, params WorkoutExerciseDto[] exercises) =>
            new(name, exercises);

        [Fact]
        public async Task Handle_WhenWorkoutDoesNotExist_ReturnsNotFound()
        {
            _workoutRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Workout?)null);

            var result = await _sut.Handle(new UpdateWorkoutCommand(1, Dto("Push Day")), CancellationToken.None);

            Assert.Equal(Errors.Workout.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            var workout = new Workout { Id = 1, UserId = 5 };
            _workoutRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(workout);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(new UpdateWorkoutCommand(1, Dto("Push Day")), CancellationToken.None);

            Assert.Equal(Errors.Workout.Unauthorized, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenValid_ReplacesNameAndExercisesEntirely()
        {
            var workout = new Workout
            {
                Id = 1,
                UserId = 5,
                Name = "Old Name",
                WorkoutExercises = [new WorkoutExercise { Id = 99, ExerciseId = 1 }]
            };
            _workoutRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(workout);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);
            var newExercise = new WorkoutExerciseDto(Order: 1, Notes: null, ExerciseId: 2, WorkoutId: 1, null, null, []);

            var result = await _sut.Handle(new UpdateWorkoutCommand(1, Dto("New Name", newExercise)), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal("New Name", workout.Name);
            Assert.Single(workout.WorkoutExercises);
            Assert.Equal(2, workout.WorkoutExercises.First().ExerciseId);
            _workoutRepository.Verify(r => r.UpdateAsync(workout, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
