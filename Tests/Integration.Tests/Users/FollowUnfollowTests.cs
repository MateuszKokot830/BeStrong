using System.Net;
using System.Net.Http.Json;
using Application.Dto.User;
using Integration.Tests.TestDoubles;

namespace Integration.Tests.Users
{
    public class FollowUnfollowTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public FollowUnfollowTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private async Task<(HttpClient Client, int UserId)> RegisterClientAsync(string username)
        {
            var client = _factory.CreateClient();
            var token = await client.RegisterAndGetTokenAsync(username);
            client.SetBearerToken(token);
            var me = await client.GetFromJsonAsync<UserDto>("/api/users/me");
            return (client, me!.Id);
        }

        [Fact]
        public async Task FollowUser_ThenUnfollow_RoundTripsCorrectly()
        {
            var (follower, followerId) = await RegisterClientAsync("kate_follower");
            var (_, followedId) = await RegisterClientAsync("kate_followed");

            var followResponse = await follower.PostAsync($"/api/users/{followerId}/follow?followUserId={followedId}", content: null);
            Assert.Equal(HttpStatusCode.NoContent, followResponse.StatusCode);

            var unfollowResponse = await follower.DeleteAsync($"/api/users/{followerId}/follow?unfollowUserId={followedId}");
            Assert.Equal(HttpStatusCode.NoContent, unfollowResponse.StatusCode);
        }

        [Fact]
        public async Task FollowUser_WhenAlreadyFollowing_IsIdempotent()
        {
            var (follower, followerId) = await RegisterClientAsync("kate_dupfollower");
            var (_, followedId) = await RegisterClientAsync("kate_dupfollowed");
            await follower.PostAsync($"/api/users/{followerId}/follow?followUserId={followedId}", content: null);

            var secondFollowResponse = await follower.PostAsync($"/api/users/{followerId}/follow?followUserId={followedId}", content: null);

            Assert.Equal(HttpStatusCode.NoContent, secondFollowResponse.StatusCode);
        }

        [Fact]
        public async Task UnfollowUser_WhenNotFollowing_IsIdempotent()
        {
            var (follower, followerId) = await RegisterClientAsync("kate_neverfollowed");
            var (_, otherId) = await RegisterClientAsync("kate_neverfollowedtarget");

            var response = await follower.DeleteAsync($"/api/users/{followerId}/follow?unfollowUserId={otherId}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task FollowUser_Self_ReturnsBadRequest()
        {
            var (client, userId) = await RegisterClientAsync("kate_selffollow");

            var response = await client.PostAsync($"/api/users/{userId}/follow?followUserId={userId}", content: null);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task FollowUser_OnBehalfOfADifferentUser_ReturnsUnauthorized()
        {
            var (_, userId) = await RegisterClientAsync("kate_impersonated");
            var (attacker, targetId) = await RegisterClientAsync("kate_impersonator");

            var response = await attacker.PostAsync($"/api/users/{userId}/follow?followUserId={targetId}", content: null);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
