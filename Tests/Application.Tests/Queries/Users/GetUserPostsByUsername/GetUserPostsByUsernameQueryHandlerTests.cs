using Application.Dto.Post;
using Application.Dto.User;
using Application.Interfaces.Searchers;
using Application.Queries.Users.GetUserPostsByUsername;
using Application.Tests.TestDoubles;
using Domain.Common;
using Domain.Errors;
using Moq;

namespace Application.Tests.Queries.Users.GetUserPostsByUsername
{
    public class GetUserPostsByUsernameQueryHandlerTests
    {
        private readonly Mock<IUserSearcher> _userSearcher = new();
        private readonly Mock<IPostSearcher> _postSearcher = new();
        private readonly GetUserPostsByUsernameQueryHandler _sut;

        public GetUserPostsByUsernameQueryHandlerTests()
        {
            _sut = new GetUserPostsByUsernameQueryHandler(_userSearcher.Object, _postSearcher.Object);
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsNotFoundWithoutQueryingPosts()
        {
            _userSearcher.Setup(s => s.FindByUsernameAsync("ghost", It.IsAny<CancellationToken>())).ReturnsAsync((UserDto?)null);

            var result = await _sut.Handle(new GetUserPostsByUsernameQuery("ghost"), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
            _postSearcher.Verify(s => s.FindByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenUserExists_ReturnsTheirPosts()
        {
            var user = UserDtoFactory.Create(7, "alice");
            _userSearcher.Setup(s => s.FindByUsernameAsync("alice", It.IsAny<CancellationToken>())).ReturnsAsync(user);
            var posts = new List<PostDto> { new(1, 7, PostType.Normal, "hi", DateTime.UtcNow, null, null, null, 0, []) };
            _postSearcher.Setup(s => s.FindByUserIdAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync(posts);

            var result = await _sut.Handle(new GetUserPostsByUsernameQuery("alice"), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
        }
    }
}
