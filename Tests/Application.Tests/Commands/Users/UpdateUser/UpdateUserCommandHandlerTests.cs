using Application.Commands.Users.UpdateUser;
using Application.Dto.User;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Common;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.Users.UpdateUser
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly UpdateUserCommandHandler _sut;

        public UpdateUserCommandHandlerTests()
        {
            _sut = new UpdateUserCommandHandler(_userRepository.Object, _currentUserService.Object);
        }

        private static UserUpdateDto Dto(int id, string? name = "New Name") => new(
            id,
            DateTime.UtcNow.AddYears(-25),
            null,
            name,
            "Surname",
            Gender.Male,
            "City",
            "Country",
            "Description",
            null,
            []);

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsNotFound()
        {
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var result = await _sut.Handle(new UpdateUserCommand(Dto(1)), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            var user = new User { Id = 5 };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(new UpdateUserCommand(Dto(5)), CancellationToken.None);

            Assert.Equal(Errors.User.Unauthorized, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenValid_AppliesDtoUpdatesTimestampAndReturnsDto()
        {
            var user = new User { Id = 5, Name = "Old Name" };
            _userRepository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var before = DateTime.UtcNow;
            var result = await _sut.Handle(new UpdateUserCommand(Dto(5, "Updated Name")), CancellationToken.None);
            var after = DateTime.UtcNow;

            Assert.False(result.IsError);
            Assert.Equal("Updated Name", user.Name);
            Assert.Equal("Updated Name", result.Value.Name);
            Assert.NotNull(user.UpdatedDate);
            Assert.InRange(user.UpdatedDate!.Value, before, after);
            _userRepository.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
