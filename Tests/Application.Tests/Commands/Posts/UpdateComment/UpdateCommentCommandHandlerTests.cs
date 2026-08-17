using Application.Commands.Posts.UpdateComment;
using Application.Dto.Comment;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Posts.UpdateComment
{
    public class UpdateCommentCommandHandlerTests
    {
        private readonly Mock<ICommentRepository> _commentRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly UpdateCommentCommandHandler _sut;

        public UpdateCommentCommandHandlerTests()
        {
            _sut = new UpdateCommentCommandHandler(_commentRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenCommentDoesNotExist_ReturnsNotFound()
        {
            _commentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Comment?)null);

            var result = await _sut.Handle(new UpdateCommentCommand(1, new UpdateCommentDto("edited")), CancellationToken.None);

            Assert.Equal(Errors.Comment.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            var comment = new Comment { Id = 1, UserId = 5 };
            _commentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(comment);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(new UpdateCommentCommand(1, new UpdateCommentDto("edited")), CancellationToken.None);

            Assert.Equal(Errors.Comment.Forbidden, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenValid_UpdatesDescriptionAndReturnsDto()
        {
            var comment = new Comment { Id = 1, UserId = 5, Description = "old" };
            _commentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(comment);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new UpdateCommentCommand(1, new UpdateCommentDto("edited")), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal("edited", comment.Description);
            Assert.Equal("edited", result.Value.Description);
            _commentRepository.Verify(r => r.UpdateAsync(comment, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
