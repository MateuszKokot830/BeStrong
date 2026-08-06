using Application.Dto.User;
using Application.Interfaces.Searchers;
using Application.Queries.Users.GetUserByUsername;
using Application.Tests.TestDoubles;
using Domain.Errors;
using Moq;

namespace Application.Tests.Queries.Users.GetUserByUsername
{
    public class GetUserByUsernameQueryHandlerTests
    {
        private readonly Mock<IUserSearcher> _userSearcher = new();
        private readonly GetUserByUsernameQueryHandler _sut;

        public GetUserByUsernameQueryHandlerTests()
        {
            _sut = new GetUserByUsernameQueryHandler(_userSearcher.Object);
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsNotFound()
        {
            _userSearcher.Setup(s => s.FindByUsernameAsync("ghost", It.IsAny<CancellationToken>())).ReturnsAsync((UserDto?)null);

            var result = await _sut.Handle(new GetUserByUsernameQuery("ghost"), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenUserExists_ReturnsIt()
        {
            var user = UserDtoFactory.Create(1, "alice");
            _userSearcher.Setup(s => s.FindByUsernameAsync("alice", It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _sut.Handle(new GetUserByUsernameQuery("alice"), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Same(user, result.Value);
        }
    }
}
