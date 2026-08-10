using Application.Commands.Workouts.CreateExercise;
using Application.Dto.Exercise;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Notifications;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Moq;

namespace Application.Tests.Commands.Workouts.CreateExercise
{
    public class CreateExerciseCommandHandlerTests
    {
        private readonly Mock<IExerciseRepository> _exerciseRepository = new();
        private readonly Mock<IPublisher> _publisher = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly CreateExerciseCommandHandler _sut;

        public CreateExerciseCommandHandlerTests()
        {
            _sut = new CreateExerciseCommandHandler(_exerciseRepository.Object, _publisher.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_CreatesExerciseAndPublishesChangeNotification()
        {
            var dto = new CreateExerciseDto("Bench Press", "Chest exercise", MuscleSubgroup.Chest, null);

            var result = await _sut.Handle(new CreateExerciseCommand(dto), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal("Bench Press", result.Value.Name);
            Assert.Equal(MuscleGroup.Chest, result.Value.MuscleGroup);
            _exerciseRepository.Verify(r => r.AddAsync(It.Is<Exercise>(e => e.Name == "Bench Press"), It.IsAny<CancellationToken>()), Times.Once);
            _publisher.Verify(p => p.Publish(It.IsAny<ExerciseChangedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CommitsBeforeBuildingTheDto_SoTheGeneratedIdIsAvailable()
        {
            var dto = new CreateExerciseDto("Bench Press", "Chest exercise", MuscleSubgroup.Chest, null);

            await _sut.Handle(new CreateExerciseCommand(dto), CancellationToken.None);

            _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
