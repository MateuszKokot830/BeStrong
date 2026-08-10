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
            // AddPhoto's action body calls file.OpenReadStream() with no null check, which reads
            // as a NullReferenceException risk if "file" is ever missing. In practice it isn't
            // reachable: WebAPI has <Nullable>enable</Nullable>, so [ApiController]'s automatic
            // model validation treats the non-nullable IFormFile parameter as implicitly required
            // and rejects the request with a clean 400 before the action method ever runs. Recorded
            // here because it contradicts what the source alone suggested — this endpoint doesn't
            // need defending, it already is.
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
