using Application.Commands.Workouts.DeleteExercise;
using Application.Interfaces.Repositories;
using Application.Notifications;
using Domain.Entities;
using Domain.Errors;
using MediatR;
using Moq;

namespace Application.Tests.Commands.Workouts.DeleteExercise
{
    public class DeleteExerciseCommandHandlerTests
    {
        private readonly Mock<IExerciseRepository> _exerciseRepository = new();
        private readonly Mock<IPublisher> _publisher = new();
        private readonly DeleteExerciseCommandHandler _sut;

        public DeleteExerciseCommandHandlerTests()
        {
            _sut = new DeleteExerciseCommandHandler(_exerciseRepository.Object, _publisher.Object);
        }

        [Fact]
        public async Task Handle_WhenExerciseDoesNotExist_ReturnsNotFoundAndDoesNotPublish()
        {
            _exerciseRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Exercise?)null);

            var result = await _sut.Handle(new DeleteExerciseCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Exercise.NotFound, result.FirstError);
            _publisher.Verify(p => p.Publish(It.IsAny<ExerciseChangedNotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenExerciseExists_DeletesItAndPublishesChangeNotification()
        {
            var exercise = new Exercise { Id = 1, Name = "Bench Press" };
            _exerciseRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(exercise);

            var result = await _sut.Handle(new DeleteExerciseCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _exerciseRepository.Verify(r => r.DeleteAsync(exercise, It.IsAny<CancellationToken>()), Times.Once);
            _publisher.Verify(p => p.Publish(It.IsAny<ExerciseChangedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
