using Application.Commands.Users.AddPhoto;
using Application.Dto.Photo;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Users.AddPhoto
{
    public class AddPhotoCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IPhotoService> _photoService = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly AddPhotoCommandHandler _sut;

        public AddPhotoCommandHandlerTests()
        {
            _sut = new AddPhotoCommandHandler(_userRepository.Object, _photoService.Object, _currentUserService.Object);
        }

        private static AddPhotoCommand Command(int userId) =>
            new(Stream.Null, "photo.jpg", 100, userId);

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsUserNotFound()
        {
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var result = await _sut.Handle(Command(1), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            var user = new User { Id = 5 };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(Command(5), CancellationToken.None);

            Assert.Equal(Errors.User.Unauthorized, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenUploadThrows_ReturnsUploadFailedAndDoesNotPersistPhoto()
        {
            var user = new User { Id = 5, Photos = [] };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);
            _photoService
                .Setup(s => s.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("cloudinary down"));

            var result = await _sut.Handle(Command(5), CancellationToken.None);

            Assert.Equal(Errors.Photo.UploadFailed, result.FirstError);
            _userRepository.Verify(r => r.AddPhotoAsync(It.IsAny<Photo>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenUserHasNoPhotosYet_NewPhotoBecomesProfilePhoto()
        {
            var user = new User { Id = 5, Photos = [] };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);
            _photoService
                .Setup(s => s.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PhotoUploadResult("http://img/1.jpg", "pub-1"));

            var result = await _sut.Handle(Command(5), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.True(result.Value.IsProfilePhoto);
            _userRepository.Verify(r => r.AddPhotoAsync(
                It.Is<Photo>(p => p.UserId == 5 && p.Url == "http://img/1.jpg" && p.IsProfilePhoto),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenUserAlreadyHasPhotos_NewPhotoIsNotProfilePhoto()
        {
            var user = new User { Id = 5, Photos = [new Photo { Id = 1, UserId = 5 }] };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);
            _photoService
                .Setup(s => s.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PhotoUploadResult("http://img/2.jpg", "pub-2"));

            var result = await _sut.Handle(Command(5), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.False(result.Value.IsProfilePhoto);
        }
    }
}
