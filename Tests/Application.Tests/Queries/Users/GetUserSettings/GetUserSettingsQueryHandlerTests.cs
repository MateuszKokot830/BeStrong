using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Application.Mappings;
using Application.Queries.Users.GetUserSettings;
using Moq;

namespace Application.Tests.Queries.Users.GetUserSettings
{
    public class GetUserSettingsQueryHandlerTests
    {
        private readonly Mock<IUserSearcher> _userSearcher = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly GetUserSettingsQueryHandler _sut;

        public GetUserSettingsQueryHandlerTests()
        {
            _sut = new GetUserSettingsQueryHandler(_userSearcher.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_ReturnsTheCurrentUsersSettings()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            _userSearcher.Setup(s => s.GetSettingsAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync(UserSettingsMappings.Default);

            var result = await _sut.Handle(new GetUserSettingsQuery(), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal(UserSettingsMappings.Default, result.Value);
        }
    }
}
