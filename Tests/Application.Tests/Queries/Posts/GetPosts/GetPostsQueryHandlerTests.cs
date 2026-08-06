using Application.Dto.Post;
using Application.Interfaces.Searchers;
using Application.Queries.Posts.GetPosts;
using Domain.Common;
using Moq;

namespace Application.Tests.Queries.Posts.GetPosts
{
    public class GetPostsQueryHandlerTests
    {
        private readonly Mock<IPostSearcher> _postSearcher = new();
        private readonly GetPostsQueryHandler _sut;

        public GetPostsQueryHandlerTests()
        {
            _sut = new GetPostsQueryHandler(_postSearcher.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAllPostsFromSearcher()
        {
            var posts = new List<PostDto> { new(1, 1, PostType.Normal, "hi", DateTime.UtcNow, null, null, null, 0, []) };
            _postSearcher.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(posts);

            var result = await _sut.Handle(new GetPostsQuery(), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
        }
    }
}
