namespace Integration.Tests
{
    public class CorsTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CorsTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Request_FromTheConfiguredOrigin_GetsAccessControlAllowOriginHeader()
        {
            // CORS is evaluated by UseCors, which runs before UseAuthentication in Program.cs, so
            // the header should be present even on a request that ultimately gets rejected by auth.
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/posts");
            request.Headers.Add("Origin", "http://localhost:4200");

            var response = await _client.SendAsync(request);

            Assert.True(response.Headers.TryGetValues("Access-Control-Allow-Origin", out var values));
            Assert.Equal("http://localhost:4200", values!.Single());
        }

        [Fact]
        public async Task Request_FromAnUntrustedOrigin_DoesNotGetAccessControlAllowOriginHeader()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/posts");
            request.Headers.Add("Origin", "http://evil.example.com");

            var response = await _client.SendAsync(request);

            Assert.False(response.Headers.TryGetValues("Access-Control-Allow-Origin", out _));
        }

        [Fact]
        public async Task PreflightRequest_FromTheConfiguredOrigin_IsAllowed()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "/api/posts");
            request.Headers.Add("Origin", "http://localhost:4200");
            request.Headers.Add("Access-Control-Request-Method", "GET");
            request.Headers.Add("Access-Control-Request-Headers", "authorization");

            var response = await _client.SendAsync(request);

            Assert.True(response.Headers.TryGetValues("Access-Control-Allow-Origin", out var origin));
            Assert.Equal("http://localhost:4200", origin!.Single());
            Assert.True(response.Headers.TryGetValues("Access-Control-Allow-Methods", out _));
        }

        [Fact]
        public async Task PreflightRequest_FromAnUntrustedOrigin_IsRejected()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "/api/posts");
            request.Headers.Add("Origin", "http://evil.example.com");
            request.Headers.Add("Access-Control-Request-Method", "GET");

            var response = await _client.SendAsync(request);

            Assert.False(response.Headers.TryGetValues("Access-Control-Allow-Origin", out _));
        }
    }
}
