using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Notifications;
using Domain.Aggregates;
using Domain.Common;
using Moq;

namespace Application.Tests.Notifications
{
    public class WorkoutSavedNotificationHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly WorkoutSavedNotificationHandler _sut;

        public WorkoutSavedNotificationHandlerTests()
        {
            _sut = new WorkoutSavedNotificationHandler(_postRepository.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_CreatesAWorkoutPublicationPostForTheWorkoutsAuthor()
        {
            var notification = new WorkoutSavedNotification(WorkoutId: 5, UserId: 7, Description: "Push Day");

            await _sut.Handle(notification, CancellationToken.None);

            _postRepository.Verify(r => r.AddAsync(
                It.Is<Post>(p =>
                    p.UserId == 7 &&
                    p.Type == PostType.WorkoutPublication &&
                    p.Description == "Push Day" &&
                    p.WorkoutId == 5 &&
                    p.WorkoutPlanId == null),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CommitsTheNewPost()
        {
            var notification = new WorkoutSavedNotification(WorkoutId: 5, UserId: 7, Description: "Push Day");

            await _sut.Handle(notification, CancellationToken.None);

            _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenWorkoutHasNoName_CreatesPostWithNullDescription()
        {
            var notification = new WorkoutSavedNotification(WorkoutId: 5, UserId: 7, Description: null);

            await _sut.Handle(notification, CancellationToken.None);

            _postRepository.Verify(r => r.AddAsync(
                It.Is<Post>(p => p.Description == null),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
