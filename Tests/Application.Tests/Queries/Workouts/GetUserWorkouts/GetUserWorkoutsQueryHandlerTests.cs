using Application.Dto.User;
using Application.Dto.Workout;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Application.Mappings;
using Application.Queries.Workouts.GetUserWorkouts;
using Domain.Common;
using Moq;

namespace Application.Tests.Queries.Workouts.GetUserWorkouts
{
    public class GetUserWorkoutsQueryHandlerTests
    {
        private readonly Mock<IWorkoutSearcher> _workoutSearcher = new();
        private readonly Mock<IUserSearcher> _userSearcher = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly GetUserWorkoutsQueryHandler _sut;

        public GetUserWorkoutsQueryHandlerTests()
        {
            _sut = new GetUserWorkoutsQueryHandler(_workoutSearcher.Object, _userSearcher.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenCallerIsOwnerOrAdmin_ReturnsWorkoutsWithoutCheckingSettings()
        {
            var workouts = new List<WorkoutDto> { new(1, 5, DateTime.UtcNow, "Push Day", []) };
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);
            _workoutSearcher.Setup(s => s.FindByUserIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(workouts);

            var result = await _sut.Handle(new GetUserWorkoutsQuery(5), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
            _userSearcher.Verify(s => s.GetSettingsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenStrangerAndWorkoutsArePrivate_ReturnsEmptyList()
        {
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);
            _currentUserService.Setup(s => s.UserId).Returns(99);
            _userSearcher.Setup(s => s.GetSettingsAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(
                new UserSettingsDto(ProfileVisibility.Public, ProfileVisibility.Private, ProfileVisibility.Public, ProfileVisibility.Public, true, true));
            _userSearcher.Setup(s => s.GetFollowedUserIdsAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync([]);

            var result = await _sut.Handle(new GetUserWorkoutsQuery(5), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Empty(result.Value);
            _workoutSearcher.Verify(s => s.FindByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenStrangerAndWorkoutsArePublicDefault_ReturnsWorkouts()
        {
            var workouts = new List<WorkoutDto> { new(1, 5, DateTime.UtcNow, "Push Day", []) };
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);
            _currentUserService.Setup(s => s.UserId).Returns(99);
            _userSearcher.Setup(s => s.GetSettingsAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(UserSettingsMappings.Default);
            _userSearcher.Setup(s => s.GetFollowedUserIdsAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync([]);
            _workoutSearcher.Setup(s => s.FindByUserIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(workouts);

            var result = await _sut.Handle(new GetUserWorkoutsQuery(5), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
        }
    }
}
