using Application.Commands.Posts.LikeComment;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Posts.LikeComment
{
    public class LikeCommentCommandHandlerTests
    {
        private readonly Mock<ICommentRepository> _commentRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly LikeCommentCommandHandler _sut;

        public LikeCommentCommandHandlerTests()
        {
            _sut = new LikeCommentCommandHandler(_commentRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenCommentDoesNotExist_ReturnsNotFound()
        {
            _commentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Comment?)null);

            var result = await _sut.Handle(new LikeCommentCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Comment.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenAlreadyLikedByCaller_ReturnsAlreadyLiked()
        {
            _currentUserService.Setup(s => s.UserId).Returns(9);
            var comment = new Comment { Id = 1, Likes = [new CommentLike { UserId = 9, CommentId = 1 }] };
            _commentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(comment);

            var result = await _sut.Handle(new LikeCommentCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Comment.AlreadyLiked, result.FirstError);
            _commentRepository.Verify(r => r.AddLikeAsync(It.IsAny<CommentLike>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenNotYetLiked_AddsLikeForCurrentUser()
        {
            _currentUserService.Setup(s => s.UserId).Returns(9);
            var comment = new Comment { Id = 1, Likes = [] };
            _commentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(comment);

            var result = await _sut.Handle(new LikeCommentCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _commentRepository.Verify(r => r.AddLikeAsync(
                It.Is<CommentLike>(l => l.UserId == 9 && l.CommentId == 1),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
