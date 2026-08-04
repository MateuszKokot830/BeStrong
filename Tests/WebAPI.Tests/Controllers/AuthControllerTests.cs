using Application.Commands.Register;
using Application.Dto.Auth;
using Application.Queries.Login;
using Domain.Errors;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Tests.TestDoubles;

namespace WebAPI.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediator = new();
        private readonly AuthController _sut;

        public AuthControllerTests()
        {
            _sut = new AuthController(_mediator.Object)
            {
                ProblemDetailsFactory = new TestProblemDetailsFactory(),
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        private static UserRegisterRequestDto ValidRegisterDto() => new("newuser", "Password1");
        private static UserLoginRequestDto ValidLoginDto() => new("existinguser", "Password1");

        private void SetupRegister(ErrorOr<UserAuthResponseDto> result) =>
            _mediator
                .Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

        private void SetupLogin(ErrorOr<UserAuthResponseDto> result) =>
            _mediator
                .Setup(m => m.Send(It.IsAny<LoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

        private static void AssertStatus(IActionResult result, int expectedStatusCode)
        {
            var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal(expectedStatusCode, objectResult.StatusCode);
        }

        // ---------------------------------------------------------------
        // Register — happy path
        // ---------------------------------------------------------------

        [Fact]
        public async Task Register_WhenSucceeds_ReturnsOkWithAuthResponse()
        {
            var expected = new UserAuthResponseDto("newuser", "jwt-token");
            SetupRegister(expected);

            var result = await _sut.Register(ValidRegisterDto());

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(expected, ok.Value);
        }

        [Fact]
        public async Task Register_SendsCommandWrappingTheRequestDto()
        {
            var dto = ValidRegisterDto();
            SetupRegister(new UserAuthResponseDto("newuser", "jwt-token"));

            await _sut.Register(dto);

            _mediator.Verify(m => m.Send(
                It.Is<RegisterCommand>(c => c.UserRegisterRequestDto == dto),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---------------------------------------------------------------
        // Register — error mapping (edge cases)
        // ---------------------------------------------------------------

        [Fact]
        public async Task Register_WhenUsernameAlreadyExists_ReturnsConflict()
        {
            SetupRegister(Errors.User.DuplicateUsername);

            var result = await _sut.Register(ValidRegisterDto());

            AssertStatus(result, StatusCodes.Status409Conflict);
        }

        [Fact]
        public async Task Register_WhenValidationFails_ReturnsBadRequest()
        {
            ErrorOr<UserAuthResponseDto> errors = new List<Error>
            {
                Error.Validation("UserName", "Username must be at least 3 characters.")
            };
            SetupRegister(errors);

            var result = await _sut.Register(new UserRegisterRequestDto("ab", "weak"));

            AssertStatus(result, StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Register_WhenValidationFails_ResponseGroupsErrorsByCode()
        {
            ErrorOr<UserAuthResponseDto> errors = new List<Error>
            {
                Error.Validation("UserName", "Username is required."),
                Error.Validation("Password", "Password must contain at least one digit.")
            };
            SetupRegister(errors);

            var result = await _sut.Register(new UserRegisterRequestDto("", ""));

            var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
            var problem = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
            Assert.True(problem.Errors.ContainsKey("UserName"));
            Assert.True(problem.Errors.ContainsKey("Password"));
        }

        [Fact]
        public async Task Register_WhenIdentityRejectsPassword_ReturnsUnprocessableEntity()
        {
            ErrorOr<UserAuthResponseDto> errors = new List<Error>
            {
                Error.Failure("PasswordRequiresNonAlphanumeric", "Passwords must have at least one non alphanumeric character.")
            };
            SetupRegister(errors);

            var result = await _sut.Register(ValidRegisterDto());

            AssertStatus(result, StatusCodes.Status422UnprocessableEntity);
        }

        [Fact]
        public async Task Register_WhenUnexpectedError_ReturnsInternalServerError()
        {
            SetupRegister(Error.Unexpected("UnhandledException", "An unexpected error occurred."));

            var result = await _sut.Register(ValidRegisterDto());

            AssertStatus(result, StatusCodes.Status500InternalServerError);
        }

        // ---------------------------------------------------------------
        // Login — happy path
        // ---------------------------------------------------------------

        [Fact]
        public async Task Login_WhenSucceeds_ReturnsOkWithAuthResponse()
        {
            var expected = new UserAuthResponseDto("existinguser", "jwt-token");
            SetupLogin(expected);

            var result = await _sut.Login(ValidLoginDto());

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(expected, ok.Value);
        }

        [Fact]
        public async Task Login_SendsQueryWrappingTheRequestDto()
        {
            var dto = ValidLoginDto();
            SetupLogin(new UserAuthResponseDto("existinguser", "jwt-token"));

            await _sut.Login(dto);

            _mediator.Verify(m => m.Send(
                It.Is<LoginQuery>(q => q.UserLoginRequestDto == dto),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---------------------------------------------------------------
        // Login — error mapping (edge cases)
        // ---------------------------------------------------------------

        [Fact]
        public async Task Login_WhenCredentialsInvalid_ReturnsUnauthorized()
        {
            SetupLogin(Errors.Auth.InvalidCredentials);

            var result = await _sut.Login(new UserLoginRequestDto("existinguser", "wrongpassword"));

            AssertStatus(result, StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task Login_WhenUserDoesNotExist_ReturnsUnauthorized()
        {
            SetupLogin(Errors.Auth.InvalidCredentials);

            var result = await _sut.Login(new UserLoginRequestDto("ghost", "Password1"));

            AssertStatus(result, StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task Login_WhenValidationFails_ReturnsBadRequest()
        {
            ErrorOr<UserAuthResponseDto> errors = new List<Error>
            {
                Error.Validation("UserName", "Username is required.")
            };
            SetupLogin(errors);

            var result = await _sut.Login(new UserLoginRequestDto("", "Password1"));

            AssertStatus(result, StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Login_WhenUnexpectedError_ReturnsInternalServerError()
        {
            SetupLogin(Error.Unexpected("UnhandledException", "An unexpected error occurred."));

            var result = await _sut.Login(ValidLoginDto());

            AssertStatus(result, StatusCodes.Status500InternalServerError);
        }
    }
}
