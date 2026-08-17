using Application.Commands.Users.FollowUser;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Users.FollowUser
{
    public class FollowUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly FollowUserCommandHandler _sut;

        public FollowUserCommandHandlerTests()
        {
            _sut = new FollowUserCommandHandler(_userRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotTheUserActingOnTheirOwnBehalf_ReturnsUnauthorized()
        {
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(1)).Returns(false);

            var result = await _sut.Handle(new FollowUserCommand(UserId: 1, FollowUserId: 2), CancellationToken.None);

            Assert.Equal(Errors.User.Forbidden, result.FirstError);
            _userRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenEitherUserDoesNotExist_ReturnsNotFound()
        {
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(1)).Returns(true);
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new User { Id = 1 });
            _userRepository.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var result = await _sut.Handle(new FollowUserCommand(UserId: 1, FollowUserId: 2), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenAlreadyFollowing_ReturnsSuccessWithoutAddingDuplicateFollower()
        {
            var followUser = new User { Id = 2 };
            var user = new User { Id = 1, FollowedUsers = [new Follower { UserId = 1, FollowedUserId = 2 }] };
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(1)).Returns(true);
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _userRepository.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(followUser);

            var result = await _sut.Handle(new FollowUserCommand(UserId: 1, FollowUserId: 2), CancellationToken.None);

            Assert.False(result.IsError);
            _userRepository.Verify(r => r.AddFollowerAsync(It.IsAny<Follower>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenNotYetFollowing_AddsFollower()
        {
            var followUser = new User { Id = 2 };
            var user = new User { Id = 1 };
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(1)).Returns(true);
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _userRepository.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(followUser);

            var result = await _sut.Handle(new FollowUserCommand(UserId: 1, FollowUserId: 2), CancellationToken.None);

            Assert.False(result.IsError);
            _userRepository.Verify(r => r.AddFollowerAsync(
                It.Is<Follower>(f => f.UserId == 1 && f.FollowedUserId == 2),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
