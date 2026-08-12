using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Dto.Auth;
using Application.Dto.User;

namespace Integration.Tests.Auth
{
    public class AuthRoundTripTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthRoundTripTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private const string Password = "Password1!";

        private async Task<UserAuthResponseDto> RegisterAsync(string username)
        {
            var response = await _client.PostAsJsonAsync("/api/auth/register", new UserRegisterRequestDto(username, Password));
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<UserAuthResponseDto>())!;
        }

        [Fact]
        public async Task Register_WithValidCredentials_ReturnsOkWithUsernameAndToken()
        {
            var response = await _client.PostAsJsonAsync("/api/auth/register", new UserRegisterRequestDto("alice_register", Password));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<UserAuthResponseDto>();
            Assert.Equal("alice_register", body!.Username);
            Assert.False(string.IsNullOrWhiteSpace(body.Token));
        }

        [Fact]
        public async Task Register_WithDuplicateUsername_ReturnsConflict()
        {
            await RegisterAsync("alice_duplicate");

            var response = await _client.PostAsJsonAsync("/api/auth/register", new UserRegisterRequestDto("alice_duplicate", Password));

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Register_WithWeakPassword_ReturnsUnprocessableEntity()
        {
            var response = await _client.PostAsJsonAsync("/api/auth/register", new UserRegisterRequestDto("alice_weak", "password1!"));

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithCorrectCredentials_ReturnsOkWithToken()
        {
            await RegisterAsync("alice_login");

            var response = await _client.PostAsJsonAsync("/api/auth/login", new UserLoginRequestDto("alice_login", Password));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<UserAuthResponseDto>();
            Assert.False(string.IsNullOrWhiteSpace(body!.Token));
        }

        [Fact]
        public async Task Login_WithWrongPassword_ReturnsUnauthorized()
        {
            await RegisterAsync("alice_wrongpass");

            var response = await _client.PostAsJsonAsync("/api/auth/login", new UserLoginRequestDto("alice_wrongpass", "SomethingElse1!"));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithUnknownUsername_ReturnsUnauthorized()
        {
            var response = await _client.PostAsJsonAsync("/api/auth/login", new UserLoginRequestDto("no_such_user", Password));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ProtectedEndpoint_WithValidToken_ReturnsTheAuthenticatedUser()
        {
            var auth = await RegisterAsync("alice_protected");

            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/users/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.Equal("alice_protected", user!.UserName);
        }

        [Fact]
        public async Task ProtectedEndpoint_WithoutToken_ReturnsUnauthorized()
        {
            var response = await _client.GetAsync("/api/users/me");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ProtectedEndpoint_WithInvalidToken_ReturnsUnauthorized()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/users/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "not-a-real-token");

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
