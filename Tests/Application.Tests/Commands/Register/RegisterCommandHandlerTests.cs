using Application.Commands.Register;
using Application.Dto.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Errors;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Application.Tests.Commands.Register
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<ITokenService> _tokenService = new();
        private readonly RegisterCommandHandler _sut;

        public RegisterCommandHandlerTests()
        {
            _sut = new RegisterCommandHandler(_userRepository.Object, _tokenService.Object);
        }

        private static RegisterCommand ValidCommand() => new(new UserRegisterRequestDto("newuser", "Password1"));

        [Fact]
        public async Task Handle_WhenUsernameAlreadyExists_ReturnsDuplicateUsername()
        {
            _userRepository
                .Setup(r => r.GetByUsernameAsync("newuser", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User { Id = 1, UserName = "newuser" });

            var result = await _sut.Handle(ValidCommand(), CancellationToken.None);

            Assert.True(result.IsError);
            Assert.Equal(Errors.User.DuplicateUsername, result.FirstError);
            _userRepository.Verify(r => r.RegisterUserAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenIdentityRegistrationFails_ReturnsFailureErrorsAndDoesNotCreateToken()
        {
            _userRepository
                .Setup(r => r.GetByUsernameAsync("newuser", It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
            _userRepository
                .Setup(r => r.RegisterUserAsync(It.IsAny<User>(), "Password1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordRequiresNonAlphanumeric",
                    Description = "Passwords must have at least one non alphanumeric character."
                }));

            var result = await _sut.Handle(ValidCommand(), CancellationToken.None);

            Assert.True(result.IsError);
            Assert.Equal("PasswordRequiresNonAlphanumeric", result.FirstError.Code);
            _tokenService.Verify(t => t.CreateTokenAsync(It.IsAny<CreateTokenRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenSucceeds_CreatesTokenAndReturnsAuthResponse()
        {
            _userRepository
                .Setup(r => r.GetByUsernameAsync("newuser", It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
            _userRepository
                .Setup(r => r.RegisterUserAsync(It.IsAny<User>(), "Password1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(IdentityResult.Success);
            _tokenService
                .Setup(t => t.CreateTokenAsync(It.IsAny<CreateTokenRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("jwt-token");

            var result = await _sut.Handle(ValidCommand(), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal("newuser", result.Value.Username);
            Assert.Equal("jwt-token", result.Value.Token);
        }

        [Fact]
        public async Task Handle_WhenSucceeds_PassesRegisteredUserIdAndUsernameToTokenService()
        {
            _userRepository
                .Setup(r => r.GetByUsernameAsync("newuser", It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
            _userRepository
                .Setup(r => r.RegisterUserAsync(It.Is<User>(u => u.UserName == "newuser"), "Password1", It.IsAny<CancellationToken>()))
                .Callback<User, string?, CancellationToken>((user, _, _) => user.Id = 42)
                .ReturnsAsync(IdentityResult.Success);

            await _sut.Handle(ValidCommand(), CancellationToken.None);

            _tokenService.Verify(t => t.CreateTokenAsync(
                It.Is<CreateTokenRequest>(r => r.UserId == 42 && r.Username == "newuser"),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
