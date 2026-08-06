using Application.Dto.User;
using Application.Interfaces.Searchers;
using Application.Queries.Users.GetUsersByIds;
using Application.Tests.TestDoubles;
using Moq;

namespace Application.Tests.Queries.Users.GetUsersByIds
{
    public class GetUsersByIdsQueryHandlerTests
    {
        private readonly Mock<IUserSearcher> _userSearcher = new();
        private readonly GetUsersByIdsQueryHandler _sut;

        public GetUsersByIdsQueryHandlerTests()
        {
            _sut = new GetUsersByIdsQueryHandler(_userSearcher.Object);
        }

        [Fact]
        public async Task Handle_ReturnsUsersMatchingRequestedIds()
        {
            var users = new List<UserDto> { UserDtoFactory.Create(1, "alice"), UserDtoFactory.Create(2, "bob") };
            _userSearcher
                .Setup(s => s.FindByIdsAsync(It.Is<IReadOnlyCollection<int>>(ids => ids.SequenceEqual(new[] { 1, 2 })), It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            var result = await _sut.Handle(new GetUsersByIdsQuery([1, 2]), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal(2, result.Value.Count());
        }
    }
}
