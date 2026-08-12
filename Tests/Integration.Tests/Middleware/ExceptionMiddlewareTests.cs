using System.Net;
using Integration.Tests.TestDoubles;

namespace Integration.Tests.Middleware
{
    public class ExceptionMiddlewareTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ExceptionMiddlewareTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddPhoto_WithNoFilePart_ReturnsBadRequest_BeforeReachingTheController()
        {
            var token = await _client.RegisterAndGetTokenAsync("carol_addphoto");
            _client.SetBearerToken(token);

            using var content = new MultipartFormDataContent
            {
                { new StringContent("unrelated"), "notAFile" }
            };
            var response = await _client.PutAsync("/api/users/1/photos", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
