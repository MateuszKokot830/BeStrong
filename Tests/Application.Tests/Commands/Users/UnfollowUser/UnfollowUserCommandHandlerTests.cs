using Application.Commands.Users.UnfollowUser;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Users.UnfollowUser
{
    public class UnfollowUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly UnfollowUserCommandHandler _sut;

        public UnfollowUserCommandHandlerTests()
        {
            _sut = new UnfollowUserCommandHandler(_userRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotTheUserActingOnTheirOwnBehalf_ReturnsUnauthorized()
        {
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(1)).Returns(false);

            var result = await _sut.Handle(new UnfollowUserCommand(UserId: 1, UnfollowUserId: 2), CancellationToken.None);

            Assert.Equal(Errors.User.Forbidden, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsNotFound()
        {
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(1)).Returns(true);
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var result = await _sut.Handle(new UnfollowUserCommand(UserId: 1, UnfollowUserId: 2), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenNotFollowing_ReturnsSuccessWithoutDeletingAnything()
        {
            var user = new User { Id = 1, FollowedUsers = [] };
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(1)).Returns(true);
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _sut.Handle(new UnfollowUserCommand(UserId: 1, UnfollowUserId: 2), CancellationToken.None);

            Assert.False(result.IsError);
            _userRepository.Verify(r => r.DeleteFollowerAsync(It.IsAny<Follower>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenFollowing_DeletesTheFollowerRecord()
        {
            var follower = new Follower { UserId = 1, FollowedUserId = 2 };
            var user = new User { Id = 1, FollowedUsers = [follower] };
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(1)).Returns(true);
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _sut.Handle(new UnfollowUserCommand(UserId: 1, UnfollowUserId: 2), CancellationToken.None);

            Assert.False(result.IsError);
            _userRepository.Verify(r => r.DeleteFollowerAsync(follower, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
