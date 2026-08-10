using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Dto.Auth;

namespace Integration.Tests.TestDoubles
{
    internal static class AuthenticatedClientExtensions
    {
        public const string SeedAdminUsername = "mateo830";
        public const string SeedAdminPassword = "Pa$$w0rd";

        public static async Task<string> RegisterAndGetTokenAsync(this HttpClient client, string username, string password = "Password1!")
        {
            var response = await client.PostAsJsonAsync("/api/auth/register", new UserRegisterRequestDto(username, password));
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadFromJsonAsync<UserAuthResponseDto>();
            return body!.Token!;
        }

        public static async Task<string> LoginAndGetTokenAsync(this HttpClient client, string username, string password)
        {
            var response = await client.PostAsJsonAsync("/api/auth/login", new UserLoginRequestDto(username, password));
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadFromJsonAsync<UserAuthResponseDto>();
            return body!.Token!;
        }

        public static void SetBearerToken(this HttpClient client, string token) =>
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
