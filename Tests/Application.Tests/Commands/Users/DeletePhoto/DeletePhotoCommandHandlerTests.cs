using Application.Commands.Users.DeletePhoto;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Users.DeletePhoto
{
    public class DeletePhotoCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IPhotoService> _photoService = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly DeletePhotoCommandHandler _sut;

        public DeletePhotoCommandHandlerTests()
        {
            _sut = new DeletePhotoCommandHandler(_userRepository.Object, _photoService.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsUserNotFound()
        {
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var result = await _sut.Handle(new DeletePhotoCommand(PhotoId: 1, UserId: 1), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            var user = new User { Id = 5 };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(new DeletePhotoCommand(PhotoId: 1, UserId: 5), CancellationToken.None);

            Assert.Equal(Errors.User.Forbidden, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenPhotoDoesNotBelongToUser_ReturnsPhotoNotFound()
        {
            var user = new User { Id = 5, Photos = [] };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new DeletePhotoCommand(PhotoId: 1, UserId: 5), CancellationToken.None);

            Assert.Equal(Errors.Photo.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenPhotoIsTheProfilePhoto_ReturnsIsProfilePhotoAndDoesNotDelete()
        {
            var photo = new Photo { Id = 1, UserId = 5, IsProfilePhoto = true, PublicId = "pub-1" };
            var user = new User { Id = 5, Photos = [photo] };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new DeletePhotoCommand(PhotoId: 1, UserId: 5), CancellationToken.None);

            Assert.Equal(Errors.Photo.IsProfilePhoto, result.FirstError);
            _photoService.Verify(s => s.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _userRepository.Verify(r => r.DeletePhotoAsync(It.IsAny<Photo>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenPhotoHasPublicId_DeletesFromPhotoServiceThenRepository()
        {
            var photo = new Photo { Id = 1, UserId = 5, IsProfilePhoto = false, PublicId = "pub-1" };
            var user = new User { Id = 5, Photos = [photo] };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new DeletePhotoCommand(PhotoId: 1, UserId: 5), CancellationToken.None);

            Assert.False(result.IsError);
            _photoService.Verify(s => s.DeleteAsync("pub-1", It.IsAny<CancellationToken>()), Times.Once);
            _userRepository.Verify(r => r.DeletePhotoAsync(photo, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenPhotoHasNoPublicId_SkipsPhotoServiceButStillDeletesFromRepository()
        {
            var photo = new Photo { Id = 1, UserId = 5, IsProfilePhoto = false, PublicId = null };
            var user = new User { Id = 5, Photos = [photo] };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new DeletePhotoCommand(PhotoId: 1, UserId: 5), CancellationToken.None);

            Assert.False(result.IsError);
            _photoService.Verify(s => s.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _userRepository.Verify(r => r.DeletePhotoAsync(photo, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
