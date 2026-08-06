using Application.Dto.User;
using Application.Interfaces.Searchers;
using Application.Queries.Users.GetUsers;
using Application.Tests.TestDoubles;
using Moq;

namespace Application.Tests.Queries.Users.GetUsers
{
    public class GetUsersQueryHandlerTests
    {
        private readonly Mock<IUserSearcher> _userSearcher = new();
        private readonly GetUsersQueryHandler _sut;

        public GetUsersQueryHandlerTests()
        {
            _sut = new GetUsersQueryHandler(_userSearcher.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAllUsersFromSearcher()
        {
            var users = new List<UserDto> { UserDtoFactory.Create(1, "alice") };
            _userSearcher.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

            var result = await _sut.Handle(new GetUsersQuery(), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
        }
    }
}
