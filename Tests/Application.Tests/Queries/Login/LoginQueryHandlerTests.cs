using Application.Dto.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Queries.Login;
using Domain.Aggregates;
using Domain.Errors;
using Moq;

namespace Application.Tests.Queries.Login
{
    public class LoginQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<ITokenService> _tokenService = new();
        private readonly LoginQueryHandler _sut;

        public LoginQueryHandlerTests()
        {
            _sut = new LoginQueryHandler(_userRepository.Object, _tokenService.Object);
        }

        private static LoginQuery ValidQuery() => new(new UserLoginRequestDto("existinguser", "Password1"));

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsInvalidCredentialsWithoutCheckingPassword()
        {
            _userRepository
                .Setup(r => r.GetByUsernameAsync("existinguser", It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var result = await _sut.Handle(ValidQuery(), CancellationToken.None);

            Assert.True(result.IsError);
            Assert.Equal(Errors.Auth.InvalidCredentials, result.FirstError);
            _userRepository.Verify(r => r.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenPasswordIsWrong_ReturnsInvalidCredentials()
        {
            var user = new User { Id = 1, UserName = "existinguser" };
            _userRepository
                .Setup(r => r.GetByUsernameAsync("existinguser", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _userRepository
                .Setup(r => r.CheckPasswordAsync(user, "Password1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _sut.Handle(ValidQuery(), CancellationToken.None);

            Assert.True(result.IsError);
            Assert.Equal(Errors.Auth.InvalidCredentials, result.FirstError);
            _tokenService.Verify(t => t.CreateTokenAsync(It.IsAny<CreateTokenRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenCredentialsAreValid_ReturnsAuthResponseWithToken()
        {
            var user = new User { Id = 7, UserName = "existinguser" };
            _userRepository
                .Setup(r => r.GetByUsernameAsync("existinguser", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _userRepository
                .Setup(r => r.CheckPasswordAsync(user, "Password1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _tokenService
                .Setup(t => t.CreateTokenAsync(It.Is<CreateTokenRequest>(r => r.UserId == 7 && r.Username == "existinguser"), It.IsAny<CancellationToken>()))
                .ReturnsAsync("jwt-token");

            var result = await _sut.Handle(ValidQuery(), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal("existinguser", result.Value.Username);
            Assert.Equal("jwt-token", result.Value.Token);
        }
    }
}
