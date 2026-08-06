using Application.Commands.Users.SetMainPhoto;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Users.SetMainPhoto
{
    public class SetMainPhotoCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly SetMainPhotoCommandHandler _sut;

        public SetMainPhotoCommandHandlerTests()
        {
            _sut = new SetMainPhotoCommandHandler(_userRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsUserNotFound()
        {
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var result = await _sut.Handle(new SetMainPhotoCommand(PhotoId: 1, UserId: 1), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            var user = new User { Id = 5 };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(new SetMainPhotoCommand(PhotoId: 1, UserId: 5), CancellationToken.None);

            Assert.Equal(Errors.User.Unauthorized, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenPhotoDoesNotBelongToUser_ReturnsPhotoNotFound()
        {
            var user = new User { Id = 5, Photos = [new Photo { Id = 1, UserId = 5 }] };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new SetMainPhotoCommand(PhotoId: 999, UserId: 5), CancellationToken.None);

            Assert.Equal(Errors.Photo.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenAnotherPhotoWasMain_DemotesItAndPromotesTheRequestedOne()
        {
            var oldMain = new Photo { Id = 1, UserId = 5, IsProfilePhoto = true };
            var newMain = new Photo { Id = 2, UserId = 5, IsProfilePhoto = false };
            var user = new User { Id = 5, Photos = [oldMain, newMain] };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new SetMainPhotoCommand(PhotoId: 2, UserId: 5), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.False(oldMain.IsProfilePhoto);
            Assert.True(newMain.IsProfilePhoto);
            _userRepository.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoPhotoWasPreviouslyMain_JustPromotesTheRequestedOne()
        {
            var photo = new Photo { Id = 2, UserId = 5, IsProfilePhoto = false };
            var user = new User { Id = 5, Photos = [photo] };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new SetMainPhotoCommand(PhotoId: 2, UserId: 5), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.True(photo.IsProfilePhoto);
        }
    }
}
