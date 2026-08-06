using Application.Commands.Posts.UnlikeComment;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Posts.UnlikeComment
{
    public class UnlikeCommentCommandHandlerTests
    {
        private readonly Mock<ICommentRepository> _commentRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly UnlikeCommentCommandHandler _sut;

        public UnlikeCommentCommandHandlerTests()
        {
            _sut = new UnlikeCommentCommandHandler(_commentRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenCommentDoesNotExist_ReturnsNotFound()
        {
            _commentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Comment?)null);

            var result = await _sut.Handle(new UnlikeCommentCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Comment.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenNotLikedByCaller_ReturnsSuccessWithoutDeletingAnything()
        {
            _currentUserService.Setup(s => s.UserId).Returns(9);
            var comment = new Comment { Id = 1, Likes = [] };
            _commentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(comment);

            var result = await _sut.Handle(new UnlikeCommentCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _commentRepository.Verify(r => r.DeleteLikeAsync(It.IsAny<CommentLike>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenLikedByCaller_DeletesTheCallersLike()
        {
            _currentUserService.Setup(s => s.UserId).Returns(9);
            var like = new CommentLike { UserId = 9, CommentId = 1 };
            var comment = new Comment { Id = 1, Likes = [like] };
            _commentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(comment);

            var result = await _sut.Handle(new UnlikeCommentCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _commentRepository.Verify(r => r.DeleteLikeAsync(like, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
