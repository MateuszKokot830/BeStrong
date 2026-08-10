using System.Net;
using System.Net.Http.Json;
using System.Text;
using Application.Dto.Photo;
using Application.Dto.User;
using Integration.Tests.TestDoubles;

namespace Integration.Tests.Users
{
    public class PhotoManagementTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PhotoManagementTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private static MultipartFormDataContent PhotoContent(string fileName = "photo.jpg")
        {
            var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("fake image bytes"));
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            return new MultipartFormDataContent { { fileContent, "file", fileName } };
        }

        private async Task<(string Token, int UserId)> RegisterAsync(string username)
        {
            var token = await _client.RegisterAndGetTokenAsync(username);
            _client.SetBearerToken(token);
            var me = await _client.GetFromJsonAsync<UserDto>("/api/users/me");
            return (token, me!.Id);
        }

        [Fact]
        public async Task AddPhoto_WithARealMultipartFile_ReturnsOkAndPersistsIt()
        {
            var (_, userId) = await RegisterAsync("hank_addphoto");

            using var content = PhotoContent();
            var response = await _client.PutAsync($"/api/users/{userId}/photos", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var photo = await response.Content.ReadFromJsonAsync<PhotoDto>();
            Assert.False(string.IsNullOrWhiteSpace(photo!.Url));
            Assert.False(string.IsNullOrWhiteSpace(photo.PublicId));
        }

        [Fact]
        public async Task AddPhoto_WhenItIsTheFirstPhoto_BecomesTheProfilePhoto()
        {
            var (_, userId) = await RegisterAsync("hank_firstphoto");

            using var content = PhotoContent();
            var response = await _client.PutAsync($"/api/users/{userId}/photos", content);
            var photo = await response.Content.ReadFromJsonAsync<PhotoDto>();

            Assert.True(photo!.IsProfilePhoto);
        }

        [Fact]
        public async Task AddPhoto_WhenAPhotoAlreadyExists_TheNewOneIsNotTheProfilePhoto()
        {
            var (_, userId) = await RegisterAsync("hank_secondphoto");
            using (var first = PhotoContent("first.jpg"))
                await _client.PutAsync($"/api/users/{userId}/photos", first);

            using var second = PhotoContent("second.jpg");
            var response = await _client.PutAsync($"/api/users/{userId}/photos", second);
            var photo = await response.Content.ReadFromJsonAsync<PhotoDto>();

            Assert.False(photo!.IsProfilePhoto);
        }

        [Fact]
        public async Task SetMainPhoto_PromotesTheChosenPhotoAndDemotesThePreviousOne()
        {
            var (_, userId) = await RegisterAsync("hank_setmain");
            using var first = PhotoContent("first.jpg");
            var firstResponse = await _client.PutAsync($"/api/users/{userId}/photos", first);
            var firstPhoto = await firstResponse.Content.ReadFromJsonAsync<PhotoDto>();
            using var second = PhotoContent("second.jpg");
            var secondResponse = await _client.PutAsync($"/api/users/{userId}/photos", second);
            var secondPhoto = await secondResponse.Content.ReadFromJsonAsync<PhotoDto>();

            var setMainResponse = await _client.PutAsync($"/api/users/{userId}/photos/{secondPhoto!.Id}", content: null);
            Assert.Equal(HttpStatusCode.NoContent, setMainResponse.StatusCode);

            var me = await _client.GetFromJsonAsync<UserDto>("/api/users/me");
            Assert.True(me!.Photos.Single(p => p.Id == secondPhoto.Id).IsProfilePhoto);
            Assert.False(me.Photos.Single(p => p.Id == firstPhoto!.Id).IsProfilePhoto);
        }

        [Fact]
        public async Task DeletePhoto_RemovesANonProfilePhoto()
        {
            var (_, userId) = await RegisterAsync("hank_deletephoto");
            using var first = PhotoContent("first.jpg");
            await _client.PutAsync($"/api/users/{userId}/photos", first);
            using var second = PhotoContent("second.jpg");
            var secondResponse = await _client.PutAsync($"/api/users/{userId}/photos", second);
            var secondPhoto = await secondResponse.Content.ReadFromJsonAsync<PhotoDto>();

            var deleteResponse = await _client.DeleteAsync($"/api/users/{userId}/photos/{secondPhoto!.Id}");

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            var me = await _client.GetFromJsonAsync<UserDto>("/api/users/me");
            Assert.DoesNotContain(me!.Photos, p => p.Id == secondPhoto.Id);
        }

        [Fact]
        public async Task DeletePhoto_WhenItIsTheProfilePhoto_ReturnsConflict()
        {
            var (_, userId) = await RegisterAsync("hank_deletemain");
            using var content = PhotoContent();
            var response = await _client.PutAsync($"/api/users/{userId}/photos", content);
            var photo = await response.Content.ReadFromJsonAsync<PhotoDto>();

            var deleteResponse = await _client.DeleteAsync($"/api/users/{userId}/photos/{photo!.Id}");

            Assert.Equal(HttpStatusCode.Conflict, deleteResponse.StatusCode);
        }
    }
}
