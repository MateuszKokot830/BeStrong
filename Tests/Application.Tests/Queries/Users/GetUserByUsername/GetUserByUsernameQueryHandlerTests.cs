using Application.Dto.User;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Application.Mappings;
using Application.Queries.Users.GetUserByUsername;
using Application.Tests.TestDoubles;
using Domain.Common;
using Domain.Errors;
using Moq;

namespace Application.Tests.Queries.Users.GetUserByUsername
{
    public class GetUserByUsernameQueryHandlerTests
    {
        private readonly Mock<IUserSearcher> _userSearcher = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly GetUserByUsernameQueryHandler _sut;

        public GetUserByUsernameQueryHandlerTests()
        {
            _sut = new GetUserByUsernameQueryHandler(_userSearcher.Object, _currentUserService.Object);
        }

        private static UserSettingsDto Settings(
            ProfileVisibility photos = ProfileVisibility.Public,
            ProfileVisibility workouts = ProfileVisibility.Public,
            ProfileVisibility workoutPlan = ProfileVisibility.Public,
            ProfileVisibility measurements = ProfileVisibility.Public) =>
            new(photos, workouts, workoutPlan, measurements, AutoPublishWorkouts: true, AutoPublishWorkoutPlanChanges: true);

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsNotFound()
        {
            _userSearcher.Setup(s => s.FindByUsernameAsync("ghost", It.IsAny<CancellationToken>())).ReturnsAsync((UserDto?)null);

            var result = await _sut.Handle(new GetUserByUsernameQuery("ghost"), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenViewerIsOwnerOrAdmin_ReturnsUnredactedDto()
        {
            var user = UserDtoFactory.Create(1, "alice");
            _userSearcher.Setup(s => s.FindByUsernameAsync("alice", It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(1)).Returns(true);

            var result = await _sut.Handle(new GetUserByUsernameQuery("alice"), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Same(user, result.Value);
            _userSearcher.Verify(s => s.GetSettingsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenStrangerAndEverythingIsPrivate_RedactsDataAndReturnsCanViewFalse()
        {
            var user = UserDtoFactory.Create(1, "alice");
            _userSearcher.Setup(s => s.FindByUsernameAsync("alice", It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(1)).Returns(false);
            _currentUserService.Setup(s => s.UserId).Returns(99);
            _userSearcher.Setup(s => s.GetSettingsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(
                Settings(ProfileVisibility.Private, ProfileVisibility.Private, ProfileVisibility.Private, ProfileVisibility.Private));
            _userSearcher.Setup(s => s.GetFollowedUserIdsAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync([]);

            var result = await _sut.Handle(new GetUserByUsernameQuery("alice"), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Empty(result.Value.Photos);
            Assert.Null(result.Value.Measurements);
            Assert.Null(result.Value.WorkoutPlanId);
            Assert.Null(result.Value.WorkoutPlanName);
            Assert.False(result.Value.CanViewPhotos);
            Assert.False(result.Value.CanViewWorkouts);
            Assert.False(result.Value.CanViewWorkoutPlan);
            Assert.False(result.Value.CanViewMeasurements);
        }

        [Fact]
        public async Task Handle_WhenFollowerAndFollowersOnly_CanView()
        {
            var user = UserDtoFactory.Create(1, "alice");
            _userSearcher.Setup(s => s.FindByUsernameAsync("alice", It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(1)).Returns(false);
            _currentUserService.Setup(s => s.UserId).Returns(99);
            _userSearcher.Setup(s => s.GetSettingsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(
                Settings(ProfileVisibility.FollowersOnly, ProfileVisibility.FollowersOnly, ProfileVisibility.FollowersOnly, ProfileVisibility.FollowersOnly));
            _userSearcher.Setup(s => s.GetFollowedUserIdsAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync([1]);

            var result = await _sut.Handle(new GetUserByUsernameQuery("alice"), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.True(result.Value.CanViewPhotos);
            Assert.True(result.Value.CanViewWorkouts);
            Assert.True(result.Value.CanViewWorkoutPlan);
            Assert.True(result.Value.CanViewMeasurements);
        }

        [Fact]
        public async Task Handle_WhenStrangerAndPublicDefault_CanView()
        {
            var user = UserDtoFactory.Create(1, "alice");
            _userSearcher.Setup(s => s.FindByUsernameAsync("alice", It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(1)).Returns(false);
            _currentUserService.Setup(s => s.UserId).Returns(99);
            _userSearcher.Setup(s => s.GetSettingsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Settings());
            _userSearcher.Setup(s => s.GetFollowedUserIdsAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync([]);

            var result = await _sut.Handle(new GetUserByUsernameQuery("alice"), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.True(result.Value.CanViewPhotos);
            Assert.True(result.Value.CanViewWorkouts);
            Assert.True(result.Value.CanViewWorkoutPlan);
            Assert.True(result.Value.CanViewMeasurements);
        }
    }
}
