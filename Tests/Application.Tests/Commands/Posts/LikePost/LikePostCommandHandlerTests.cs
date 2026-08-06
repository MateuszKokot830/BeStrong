using Application.Commands.Posts.LikePost;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Posts.LikePost
{
    public class LikePostCommandHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly LikePostCommandHandler _sut;

        public LikePostCommandHandlerTests()
        {
            _sut = new LikePostCommandHandler(_postRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenPostDoesNotExist_ReturnsNotFound()
        {
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Post?)null);

            var result = await _sut.Handle(new LikePostCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Post.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenAlreadyLikedByCaller_ReturnsAlreadyLiked()
        {
            _currentUserService.Setup(s => s.UserId).Returns(9);
            var post = new Post { Id = 1, Likes = [new PostLike { UserId = 9, PostId = 1 }] };
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);

            var result = await _sut.Handle(new LikePostCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Post.AlreadyLiked, result.FirstError);
            _postRepository.Verify(r => r.AddLikeAsync(It.IsAny<PostLike>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenNotYetLiked_AddsLikeForCurrentUser()
        {
            _currentUserService.Setup(s => s.UserId).Returns(9);
            var post = new Post { Id = 1, Likes = [] };
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);

            var result = await _sut.Handle(new LikePostCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _postRepository.Verify(r => r.AddLikeAsync(
                It.Is<PostLike>(l => l.UserId == 9 && l.PostId == 1),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
