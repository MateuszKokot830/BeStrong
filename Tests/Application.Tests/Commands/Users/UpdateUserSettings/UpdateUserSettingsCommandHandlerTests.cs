using Application.Commands.Users.UpdateUserSettings;
using Application.Dto.User;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Common;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Users.UpdateUserSettings
{
    public class UpdateUserSettingsCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly UpdateUserSettingsCommandHandler _sut;

        public UpdateUserSettingsCommandHandlerTests()
        {
            _sut = new UpdateUserSettingsCommandHandler(_userRepository.Object, _currentUserService.Object);
        }

        private static UserSettingsDto Dto() => new(
            ProfileVisibility.Private, ProfileVisibility.FollowersOnly, ProfileVisibility.Private, ProfileVisibility.Public,
            AutoPublishWorkouts: false, AutoPublishWorkoutPlanChanges: false);

        [Fact]
        public async Task Handle_WhenCurrentUserDoesNotExist_ReturnsNotFound()
        {
            _currentUserService.Setup(s => s.UserId).Returns(5);
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var result = await _sut.Handle(new UpdateUserSettingsCommand(Dto()), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenValid_OverwritesSettingsAndReturnsDto()
        {
            var user = new User { Id = 5 };
            _currentUserService.Setup(s => s.UserId).Returns(5);
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _sut.Handle(new UpdateUserSettingsCommand(Dto()), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal(ProfileVisibility.Private, user.Settings!.PhotosVisibility);
            Assert.Equal(ProfileVisibility.FollowersOnly, user.Settings.WorkoutsVisibility);
            Assert.False(user.Settings.AutoPublishWorkouts);
            Assert.Equal(ProfileVisibility.Private, result.Value.PhotosVisibility);
            _userRepository.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
