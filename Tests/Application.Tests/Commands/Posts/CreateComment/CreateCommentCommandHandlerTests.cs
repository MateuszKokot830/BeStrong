using Application.Commands.Posts.CreateComment;
using Application.Dto.Comment;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Moq;

namespace Application.Tests.Commands.Posts.CreateComment
{
    public class CreateCommentCommandHandlerTests
    {
        private readonly Mock<ICommentRepository> _commentRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly CreateCommentCommandHandler _sut;

        public CreateCommentCommandHandlerTests()
        {
            _sut = new CreateCommentCommandHandler(_commentRepository.Object, _currentUserService.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_CreatesCommentForCurrentUserAndPersistsIt()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            var dto = new CommentCreateDto("nice one", PostId: 3);

            var result = await _sut.Handle(new CreateCommentCommand(dto), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal(7, result.Value.UserId);
            Assert.Equal("nice one", result.Value.Description);
            Assert.Equal(3, result.Value.PostId);
            _commentRepository.Verify(r => r.AddAsync(
                It.Is<Comment>(c => c.UserId == 7 && c.PostId == 3),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CommitsBeforeBuildingTheDto_SoTheGeneratedIdIsAvailable()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            var dto = new CommentCreateDto("nice one", PostId: 3);

            await _sut.Handle(new CreateCommentCommand(dto), CancellationToken.None);

            _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
