using System.Net;
using Integration.Tests.TestDoubles;

namespace Integration.Tests.Authorization
{
    public class RoleEnforcementTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public RoleEnforcementTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetUsers_AsNonAdmin_ReturnsForbidden()
        {
            var token = await _client.RegisterAndGetTokenAsync("bob_nonadmin");
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/api/users");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetUsers_AsSeededAdmin_ReturnsOk()
        {
            // The first user seeded on startup is granted the Admin role (see SeedData.SeedRolesAsync).
            // Logging in as them proves [Authorize(Roles = Roles.Admin)] recognizes a real role claim
            // issued by TokenService, not just that unauthenticated/wrong-role callers get rejected.
            var token = await _client.LoginAndGetTokenAsync(
                AuthenticatedClientExtensions.SeedAdminUsername, AuthenticatedClientExtensions.SeedAdminPassword);
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/api/users");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetUsers_WithoutAuthentication_ReturnsUnauthorized()
        {
            var response = await _client.GetAsync("/api/users");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
