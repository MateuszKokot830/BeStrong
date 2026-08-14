using Application.Dto.Post;
using Application.Interfaces.Searchers;
using Application.Queries.Posts.GetPostById;
using Domain.Common;
using Domain.Errors;
using Moq;

namespace Application.Tests.Queries.Posts.GetPostById
{
    public class GetPostByIdQueryHandlerTests
    {
        private readonly Mock<IPostSearcher> _postSearcher = new();
        private readonly GetPostByIdQueryHandler _sut;

        public GetPostByIdQueryHandlerTests()
        {
            _sut = new GetPostByIdQueryHandler(_postSearcher.Object);
        }

        [Fact]
        public async Task Handle_WhenPostDoesNotExist_ReturnsNotFound()
        {
            _postSearcher.Setup(s => s.FindByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((PostDto?)null);

            var result = await _sut.Handle(new GetPostByIdQuery(1), CancellationToken.None);

            Assert.Equal(Errors.Post.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenPostExists_ReturnsIt()
        {
            var post = new PostDto(1, 1, PostType.Normal, "hi", DateTime.UtcNow, null, null, null, null, 0, []);
            _postSearcher.Setup(s => s.FindByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);

            var result = await _sut.Handle(new GetPostByIdQuery(1), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Same(post, result.Value);
        }
    }
}
