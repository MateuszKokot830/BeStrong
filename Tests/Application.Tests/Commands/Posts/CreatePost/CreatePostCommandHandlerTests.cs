using Application.Commands.Posts.CreatePost;
using Application.Dto.Post;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Common;
using Moq;

namespace Application.Tests.Commands.Posts.CreatePost
{
    public class CreatePostCommandHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly CreatePostCommandHandler _sut;

        public CreatePostCommandHandlerTests()
        {
            _sut = new CreatePostCommandHandler(_postRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_CreatesPostForCurrentUserAndPersistsIt()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            var dto = new PostCreateDto(PostType.Normal, "hello world", null, null);

            var result = await _sut.Handle(new CreatePostCommand(dto), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal(7, result.Value.UserId);
            Assert.Equal("hello world", result.Value.Description);
            _postRepository.Verify(r => r.AddAsync(It.Is<Post>(p => p.UserId == 7 && p.Description == "hello world"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_MapsWorkoutPublicationFields()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            var dto = new PostCreateDto(PostType.WorkoutPublication, null, WorkoutId: 3, WorkoutPlan: 4);

            var result = await _sut.Handle(new CreatePostCommand(dto), CancellationToken.None);

            Assert.Equal(PostType.WorkoutPublication, result.Value.Type);
            Assert.Equal(3, result.Value.WorkoutId);
            Assert.Equal(4, result.Value.WorkoutPlanId);
        }
    }
}
