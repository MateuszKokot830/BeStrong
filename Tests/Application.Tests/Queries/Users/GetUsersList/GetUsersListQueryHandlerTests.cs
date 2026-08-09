using Application.Dto.User;
using Application.Helpers;
using Application.Helpers.Criteria;
using Application.Interfaces.Searchers;
using Application.Queries.Users.GetUsersList;
using Application.Tests.TestDoubles;
using Moq;

namespace Application.Tests.Queries.Users.GetUsersList
{
    public class GetUsersListQueryHandlerTests
    {
        private readonly Mock<IUserSearcher> _userSearcher = new();
        private readonly GetUsersListQueryHandler _sut;

        public GetUsersListQueryHandlerTests()
        {
            _sut = new GetUsersListQueryHandler(_userSearcher.Object);
        }

        [Fact]
        public async Task Handle_PassesCriteriaThroughAndReturnsPagedResult()
        {
            var criteria = new UserSearchCriteria { ExcludeUsername = "ali", PageNumber = 2, PageSize = 10 };
            var page = new PaginationList<UserDto>([UserDtoFactory.Create(1, "alice")], count: 1, pageNumber: 2, pageSize: 10);
            _userSearcher.Setup(s => s.GetPagedAsync(criteria, It.IsAny<CancellationToken>())).ReturnsAsync(page);

            var result = await _sut.Handle(new GetUsersListQuery(criteria), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Same(page, result.Value);
        }
    }
}
