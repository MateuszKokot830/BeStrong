using System.Net;
using System.Net.Http.Json;
using Application.Dto.Post;
using Domain.Common;
using Integration.Tests.TestDoubles;

namespace Integration.Tests.Posts
{
    public class PostCrudRoundTripTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public PostCrudRoundTripTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreatePost_ReturnsTheRealDatabaseAssignedId()
        {
            // Regression test for a bug where CreatePostCommandHandler built its response DTO
            // before SaveChanges ran, so every created post came back with id: 0. The handler now
            // commits via IUnitOfWork before calling ToDto(); this proves the id in the create
            // response actually matches what a fresh read of the same post returns.
            var token = await _client.RegisterAndGetTokenAsync("gina_idzero");
            _client.SetBearerToken(token);

            var response = await _client.PostAsJsonAsync("/api/posts",
                new PostCreateDto(PostType.Normal, "id zero repro", null, null));
            var created = await response.Content.ReadFromJsonAsync<PostDto>();

            Assert.NotEqual(0, created!.Id);

            var getResponse = await _client.GetAsync($"/api/posts/{created.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        }

        [Fact]
        public async Task CreateGetUpdateDelete_RoundTripsThroughTheRealDatabase()
        {
            var token = await _client.RegisterAndGetTokenAsync("dave_postcrud");
            _client.SetBearerToken(token);

            var createResponse = await _client.PostAsJsonAsync("/api/posts",
                new PostCreateDto(PostType.Normal, "hello world", null, null));
            Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<PostDto>();

            var getResponse = await _client.GetAsync($"/api/posts/{created!.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var fetched = await getResponse.Content.ReadFromJsonAsync<PostDto>();
            Assert.Equal(created.Id, fetched!.Id);
            Assert.Equal("hello world", fetched.Description);

            var updateResponse = await _client.PutAsJsonAsync($"/api/posts/{created.Id}", new UpdatePostDto("updated text"));
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
            var updated = await updateResponse.Content.ReadFromJsonAsync<PostDto>();
            Assert.Equal("updated text", updated!.Description);

            var deleteResponse = await _client.DeleteAsync($"/api/posts/{created.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getAfterDeleteResponse = await _client.GetAsync($"/api/posts/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getAfterDeleteResponse.StatusCode);
        }

        [Fact]
        public async Task DeletePost_AsADifferentAuthenticatedUser_ReturnsForbidden()
        {
            // Proves ownership enforcement works with a real JWT-derived ICurrentUserService,
            // not just a mocked one — the same check is already unit-tested in Application.Tests,
            // but only this proves the claim actually round-trips through a real token.
            var ownerToken = await _client.RegisterAndGetTokenAsync("erin_owner");
            _client.SetBearerToken(ownerToken);
            var createResponse = await _client.PostAsJsonAsync("/api/posts",
                new PostCreateDto(PostType.Normal, "owned by erin", null, null));
            var created = await createResponse.Content.ReadFromJsonAsync<PostDto>();

            using var strangerClient = _factory.CreateClient();
            var strangerToken = await strangerClient.RegisterAndGetTokenAsync("frank_stranger");
            strangerClient.SetBearerToken(strangerToken);

            var deleteResponse = await strangerClient.DeleteAsync($"/api/posts/{created!.Id}");

            Assert.Equal(HttpStatusCode.Forbidden, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task GetPostById_WhenPostDoesNotExist_ReturnsNotFound()
        {
            var token = await _client.RegisterAndGetTokenAsync("frank_notfound");
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/api/posts/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
