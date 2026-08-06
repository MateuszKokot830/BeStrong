using Application.Commands.Workouts.DeleteWorkout;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Workouts.DeleteWorkout
{
    public class DeleteWorkoutCommandHandlerTests
    {
        private readonly Mock<IWorkoutRepository> _workoutRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly DeleteWorkoutCommandHandler _sut;

        public DeleteWorkoutCommandHandlerTests()
        {
            _sut = new DeleteWorkoutCommandHandler(_workoutRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenWorkoutDoesNotExist_ReturnsNotFound()
        {
            _workoutRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Workout?)null);

            var result = await _sut.Handle(new DeleteWorkoutCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Workout.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            var workout = new Workout { Id = 1, UserId = 5 };
            _workoutRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(workout);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(new DeleteWorkoutCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Workout.Unauthorized, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenWorkoutHasNullUserId_TreatsOwnerCheckAsForUserZero()
        {
            var workout = new Workout { Id = 1, UserId = null };
            _workoutRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(workout);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(0)).Returns(true);

            var result = await _sut.Handle(new DeleteWorkoutCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _currentUserService.Verify(s => s.IsOwnerOrAdmin(0), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCallerIsOwnerOrAdmin_DeletesWorkout()
        {
            var workout = new Workout { Id = 1, UserId = 5 };
            _workoutRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(workout);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new DeleteWorkoutCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _workoutRepository.Verify(r => r.DeleteAsync(workout, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
