using System.Net.Http.Json;
using System.Text.Json;
using Application.Dto.User;
using Integration.Tests.TestDoubles;

namespace Integration.Tests.Users
{
    public class PaginationRoundTripTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private static readonly JsonSerializerOptions CamelCase = new() { PropertyNameCaseInsensitive = true };

        public PaginationRoundTripTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private record PaginationHeaderDto(int CurrentPage, int ItemsPerPage, int TotalItems, int TotalPages);

        private static PaginationHeaderDto ReadPaginationHeader(HttpResponseMessage response)
        {
            var raw = response.Headers.GetValues("Pagination").Single();
            return JsonSerializer.Deserialize<PaginationHeaderDto>(raw, CamelCase)!;
        }

        [Fact]
        public async Task GetUsersList_ReturnsAPaginationHeaderConsistentWithTheBody()
        {
            var token = await _client.RegisterAndGetTokenAsync("page_baseline");
            for (var i = 0; i < 4; i++)
                await _client.RegisterAndGetTokenAsync($"page_user_{i}");
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/api/users/list?pageNumber=1&pageSize=2");
            response.EnsureSuccessStatusCode();

            var header = ReadPaginationHeader(response);
            var body = await response.Content.ReadFromJsonAsync<List<UserDto>>();

            Assert.Equal(1, header.CurrentPage);
            Assert.Equal(2, header.ItemsPerPage);
            Assert.Equal(2, body!.Count);
            Assert.True(header.TotalItems >= 5, $"Expected at least the 5 registered users, got {header.TotalItems}");
            Assert.Equal((int)Math.Ceiling(header.TotalItems / 2.0), header.TotalPages);
        }

        [Fact]
        public async Task GetUsersList_DifferentPages_ReturnDisjointResults()
        {
            var token = await _client.RegisterAndGetTokenAsync("page_disjoint_base");
            _client.SetBearerToken(token);
            for (var i = 0; i < 4; i++)
                await _client.RegisterAndGetTokenAsync($"page_disjoint_{i}");

            var page1Response = await _client.GetAsync("/api/users/list?pageNumber=1&pageSize=2");
            var page1 = await page1Response.Content.ReadFromJsonAsync<List<UserDto>>();
            var page2Response = await _client.GetAsync("/api/users/list?pageNumber=2&pageSize=2");
            var page2 = await page2Response.Content.ReadFromJsonAsync<List<UserDto>>();

            var page1Ids = page1!.Select(u => u.Id).ToHashSet();
            var page2Ids = page2!.Select(u => u.Id).ToHashSet();
            Assert.Empty(page1Ids.Intersect(page2Ids));
        }

        [Fact]
        public async Task GetUsersList_WithExcludeUsername_OmitsThatUserFromTheResults()
        {
            var token = await _client.RegisterAndGetTokenAsync("page_excluder");
            _client.SetBearerToken(token);
            await _client.RegisterAndGetTokenAsync("page_excluded_user");

            var response = await _client.GetAsync("/api/users/list?pageNumber=1&pageSize=50&excludeUsername=page_excluded_user");
            var body = await response.Content.ReadFromJsonAsync<List<UserDto>>();

            Assert.DoesNotContain(body!, u => u.UserName == "page_excluded_user");
        }

        [Fact]
        public async Task GetUsersList_WithoutAuthentication_ReturnsUnauthorized()
        {
            var response = await _client.GetAsync("/api/users/list?pageNumber=1&pageSize=10");

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
