using Application.Commands.Posts.UpdatePost;
using Application.Dto.Post;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Common;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Posts.UpdatePost
{
    public class UpdatePostCommandHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly UpdatePostCommandHandler _sut;

        public UpdatePostCommandHandlerTests()
        {
            _sut = new UpdatePostCommandHandler(_postRepository.Object, _currentUserService.Object);
        }

        private static UpdatePostCommand Command(int postId, string? description) =>
            new(postId, new UpdatePostDto(description));

        [Fact]
        public async Task Handle_WhenPostDoesNotExist_ReturnsNotFound()
        {
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Post?)null);

            var result = await _sut.Handle(Command(1, "new text"), CancellationToken.None);

            Assert.Equal(Errors.Post.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            var post = new Post { Id = 1, UserId = 5, Type = PostType.Normal };
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(Command(1, "new text"), CancellationToken.None);

            Assert.Equal(Errors.Post.Forbidden, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenNormalPostAndDescriptionIsNull_ReturnsDescriptionRequired()
        {
            var post = new Post { Id = 1, UserId = 5, Type = PostType.Normal };
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(Command(1, null), CancellationToken.None);

            Assert.Equal(Errors.Post.DescriptionRequired, result.FirstError);
            _postRepository.Verify(r => r.UpdateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenWorkoutPublicationAndDescriptionIsNull_IsAllowed()
        {
            var post = new Post { Id = 1, UserId = 5, Type = PostType.WorkoutPublication };
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(Command(1, null), CancellationToken.None);

            Assert.False(result.IsError);
            _postRepository.Verify(r => r.UpdateAsync(post, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenValid_UpdatesDescriptionAndUpdatedDateAndReturnsDto()
        {
            var post = new Post { Id = 1, UserId = 5, Type = PostType.Normal, Description = "old" };
            _postRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var before = DateTime.UtcNow;
            var result = await _sut.Handle(Command(1, "updated text"), CancellationToken.None);
            var after = DateTime.UtcNow;

            Assert.False(result.IsError);
            Assert.Equal("updated text", post.Description);
            Assert.NotNull(post.UpdatedDate);
            Assert.InRange(post.UpdatedDate!.Value, before, after);
            Assert.Equal("updated text", result.Value.Description);
        }
    }
}
