using Application.Commands.Posts.DeleteComment;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Posts.DeleteComment
{
    public class DeleteCommentCommandHandlerTests
    {
        private readonly Mock<ICommentRepository> _commentRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly DeleteCommentCommandHandler _sut;

        public DeleteCommentCommandHandlerTests()
        {
            _sut = new DeleteCommentCommandHandler(_commentRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenCommentDoesNotExist_ReturnsNotFound()
        {
            _commentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Comment?)null);

            var result = await _sut.Handle(new DeleteCommentCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Comment.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            var comment = new Comment { Id = 1, UserId = 5 };
            _commentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(comment);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(new DeleteCommentCommand(1), CancellationToken.None);

            Assert.Equal(Errors.Comment.Unauthorized, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsOwnerOrAdmin_DeletesComment()
        {
            var comment = new Comment { Id = 1, UserId = 5 };
            _commentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(comment);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new DeleteCommentCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _commentRepository.Verify(r => r.DeleteAsync(comment, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
