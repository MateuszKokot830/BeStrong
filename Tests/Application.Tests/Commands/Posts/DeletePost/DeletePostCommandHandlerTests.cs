using Application.Commands.Posts.DeletePost;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Posts.DeletePost
{
    public class DeletePostCommandHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly DeletePostCommandHandler _sut;

        public DeletePostCommandHandlerTests()
        {
            _sut = new DeletePostCommandHandler(_postRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenPostDoesNotExist_ReturnsNotFound()
        {
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Post?)null);

            var result = await _sut.Handle(new DeletePostCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Post.NotFound, result.FirstError);
            _postRepository.Verify(r => r.DeleteAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            var post = new Post { Id = 1, UserId = 5 };
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(new DeletePostCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Post.Unauthorized, result.FirstError);
            _postRepository.Verify(r => r.DeleteAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenCallerIsOwnerOrAdmin_DeletesPost()
        {
            var post = new Post { Id = 1, UserId = 5 };
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new DeletePostCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _postRepository.Verify(r => r.DeleteAsync(post, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
