using Application.Commands.Workouts.CreateWorkout;
using Application.Dto.Workout;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Moq;

namespace Application.Tests.Commands.Workouts.CreateWorkout
{
    public class CreateWorkoutCommandHandlerTests
    {
        private readonly Mock<IWorkoutRepository> _workoutRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly CreateWorkoutCommandHandler _sut;

        public CreateWorkoutCommandHandlerTests()
        {
            _sut = new CreateWorkoutCommandHandler(_workoutRepository.Object, _currentUserService.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_CreatesWorkoutForCurrentUserWithGivenExercisesAndPersistsIt()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            var exercise = new WorkoutExerciseDto(Order: 1, Notes: null, ExerciseId: 3, WorkoutId: 0, null, null, []);
            var dto = new CreateWorkoutDto("Push Day", [exercise]);

            var result = await _sut.Handle(new CreateWorkoutCommand(dto), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal(7, result.Value.UserId);
            Assert.Equal("Push Day", result.Value.Name);
            Assert.Single(result.Value.WorkoutExercises);
            _workoutRepository.Verify(r => r.AddAsync(
                It.Is<Workout>(w => w.UserId == 7 && w.Name == "Push Day" && w.WorkoutExercises.Count == 1),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CommitsBeforeBuildingTheDto_SoTheGeneratedIdIsAvailable()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            var dto = new CreateWorkoutDto("Push Day", []);

            await _sut.Handle(new CreateWorkoutCommand(dto), CancellationToken.None);

            _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
