using Application.Dto.User;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Application.Queries.Users.GetCurrentUser;
using Application.Tests.TestDoubles;
using Domain.Errors;
using Moq;

namespace Application.Tests.Queries.Users.GetCurrentUser
{
    public class GetCurrentUserQueryHandlerTests
    {
        private readonly Mock<IUserSearcher> _userSearcher = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly GetCurrentUserQueryHandler _sut;

        public GetCurrentUserQueryHandlerTests()
        {
            _sut = new GetCurrentUserQueryHandler(_userSearcher.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenCurrentUserDoesNotExist_ReturnsNotFound()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            _userSearcher.Setup(s => s.FindByIdAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync((UserDto?)null);

            var result = await _sut.Handle(new GetCurrentUserQuery(), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCurrentUserExists_ReturnsIt()
        {
            var user = UserDtoFactory.Create(7, "alice");
            _currentUserService.Setup(s => s.UserId).Returns(7);
            _userSearcher.Setup(s => s.FindByIdAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _sut.Handle(new GetCurrentUserQuery(), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Same(user, result.Value);
        }
    }
}
