using Application.Commands.Workouts.UpdateExercise;
using Application.Dto.Exercise;
using Application.Interfaces.Repositories;
using Application.Notifications;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;
using MediatR;
using Moq;

namespace Application.Tests.Commands.Workouts.UpdateExercise
{
    public class UpdateExerciseCommandHandlerTests
    {
        private readonly Mock<IExerciseRepository> _exerciseRepository = new();
        private readonly Mock<IPublisher> _publisher = new();
        private readonly UpdateExerciseCommandHandler _sut;

        public UpdateExerciseCommandHandlerTests()
        {
            _sut = new UpdateExerciseCommandHandler(_exerciseRepository.Object, _publisher.Object);
        }

        [Fact]
        public async Task Handle_WhenExerciseDoesNotExist_ReturnsNotFoundAndDoesNotPublish()
        {
            _exerciseRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Exercise?)null);

            var dto = new CreateExerciseDto("Bench Press", null, MuscleSubgroup.Chest, null);
            var result = await _sut.Handle(new UpdateExerciseCommand(1, dto), CancellationToken.None);

            Assert.Equal(Errors.Exercise.NotFound, result.FirstError);
            _publisher.Verify(p => p.Publish(It.IsAny<ExerciseChangedNotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenValid_UpdatesAllFieldsAndPublishesChangeNotification()
        {
            var exercise = new Exercise { Id = 1, Name = "Old", MuscleSubgroup = MuscleSubgroup.Biceps };
            _exerciseRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(exercise);
            var dto = new CreateExerciseDto("New Name", "New Desc", MuscleSubgroup.Triceps, "img.png");

            var result = await _sut.Handle(new UpdateExerciseCommand(1, dto), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal("New Name", exercise.Name);
            Assert.Equal("New Desc", exercise.Description);
            Assert.Equal(MuscleSubgroup.Triceps, exercise.MuscleSubgroup);
            Assert.Equal("img.png", exercise.ImageUrl);
            _exerciseRepository.Verify(r => r.UpdateAsync(exercise, It.IsAny<CancellationToken>()), Times.Once);
            _publisher.Verify(p => p.Publish(It.IsAny<ExerciseChangedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
