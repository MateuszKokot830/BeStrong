using Application.Dto.Post;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Application.Queries.Posts.GetFollowedUsersPosts;
using Domain.Common;
using Moq;

namespace Application.Tests.Queries.Posts.GetFollowedUsersPosts
{
    public class GetFollowedUsersPostsQueryHandlerTests
    {
        private readonly Mock<IPostSearcher> _postSearcher = new();
        private readonly Mock<IUserSearcher> _userSearcher = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly GetFollowedUsersPostsQueryHandler _sut;

        public GetFollowedUsersPostsQueryHandlerTests()
        {
            _sut = new GetFollowedUsersPostsQueryHandler(_postSearcher.Object, _userSearcher.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_LooksUpFollowedUserIdsForCurrentUserThenReturnsTheirPosts()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            _userSearcher.Setup(s => s.GetFollowedUserIdsAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync([2, 3]);
            var posts = new List<PostDto> { new(1, 2, PostType.Normal, "hi", DateTime.UtcNow, null, null, null, null, 0, []) };
            _postSearcher
                .Setup(s => s.FindByUserIdsAsync(It.Is<IReadOnlyCollection<int>>(ids => ids.SequenceEqual(new[] { 2, 3 })), It.IsAny<CancellationToken>()))
                .ReturnsAsync(posts);

            var result = await _sut.Handle(new GetFollowedUsersPostsQuery(), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
        }
    }
}
