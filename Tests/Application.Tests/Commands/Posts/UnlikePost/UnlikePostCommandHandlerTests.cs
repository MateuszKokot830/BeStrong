using Application.Commands.Posts.UnlikePost;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Posts.UnlikePost
{
    public class UnlikePostCommandHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly UnlikePostCommandHandler _sut;

        public UnlikePostCommandHandlerTests()
        {
            _sut = new UnlikePostCommandHandler(_postRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenPostDoesNotExist_ReturnsNotFound()
        {
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Post?)null);

            var result = await _sut.Handle(new UnlikePostCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Post.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenNotLikedByCaller_ReturnsSuccessWithoutDeletingAnything()
        {
            _currentUserService.Setup(s => s.UserId).Returns(9);
            var post = new Post { Id = 1, Likes = [] };
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);

            var result = await _sut.Handle(new UnlikePostCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _postRepository.Verify(r => r.DeleteLikeAsync(It.IsAny<PostLike>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenLikedByCaller_DeletesTheCallersLike()
        {
            _currentUserService.Setup(s => s.UserId).Returns(9);
            var like = new PostLike { UserId = 9, PostId = 1 };
            var post = new Post { Id = 1, Likes = [like, new PostLike { UserId = 3, PostId = 1 }] };
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);

            var result = await _sut.Handle(new UnlikePostCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _postRepository.Verify(r => r.DeleteLikeAsync(like, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
