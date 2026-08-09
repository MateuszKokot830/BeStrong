using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Dto.Auth;
using Domain.Aggregates;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace Infrastructure.Tests.Services
{
    public class TokenServiceTests
    {
        private const string TestSecret = "this-is-a-sufficiently-long-test-signing-key-1234567890";

        private static Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        }

        private static IConfiguration Config(params (string Key, string? Value)[] entries) =>
            new ConfigurationBuilder().AddInMemoryCollection(
                entries.Select(e => new KeyValuePair<string, string?>(e.Key, e.Value))).Build();

        private static JwtSecurityToken Decode(string token) => new JwtSecurityTokenHandler().ReadJwtToken(token);

        [Fact]
        public async Task CreateTokenAsync_IncludesNameIdentifierAndNameClaims()
        {
            var userManager = MockUserManager();
            userManager.Setup(m => m.FindByNameAsync("alice")).ReturnsAsync((User?)null);
            var sut = new TokenService(userManager.Object, Config(("Jwt:Secret", TestSecret)));

            var token = await sut.CreateTokenAsync(new CreateTokenRequest(7, "alice"));

            var jwt = Decode(token!);
            Assert.Equal("7", jwt.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Assert.Equal("alice", jwt.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
        }

        [Fact]
        public async Task CreateTokenAsync_WhenIdentityUserFoundWithRoles_IncludesRoleClaims()
        {
            var user = new User { Id = 7, UserName = "alice" };
            var userManager = MockUserManager();
            userManager.Setup(m => m.FindByNameAsync("alice")).ReturnsAsync(user);
            userManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(["Admin", "Member"]);
            var sut = new TokenService(userManager.Object, Config(("Jwt:Secret", TestSecret)));

            var token = await sut.CreateTokenAsync(new CreateTokenRequest(7, "alice"));

            var jwt = Decode(token!);
            var roles = jwt.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            Assert.Equal(["Admin", "Member"], roles);
        }

        [Fact]
        public async Task CreateTokenAsync_WhenIdentityUserNotFound_HasNoRoleClaimsAndSkipsRoleLookup()
        {
            var userManager = MockUserManager();
            userManager.Setup(m => m.FindByNameAsync("ghost")).ReturnsAsync((User?)null);
            var sut = new TokenService(userManager.Object, Config(("Jwt:Secret", TestSecret)));

            var token = await sut.CreateTokenAsync(new CreateTokenRequest(1, "ghost"));

            var jwt = Decode(token!);
            Assert.DoesNotContain(jwt.Claims, c => c.Type == ClaimTypes.Role);
            userManager.Verify(m => m.GetRolesAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task CreateTokenAsync_WhenJwtSecretConfigured_SignsWithIt()
        {
            var userManager = MockUserManager();
            userManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            var sut = new TokenService(userManager.Object, Config(("Jwt:Secret", TestSecret)));

            var token = await sut.CreateTokenAsync(new CreateTokenRequest(1, "alice"));

            AssertSignedWith(token!, TestSecret);
        }

        [Fact]
        public async Task CreateTokenAsync_WhenJwtSecretMissing_FallsBackToTokenKey()
        {
            var userManager = MockUserManager();
            userManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            var sut = new TokenService(userManager.Object, Config(("TokenKey", TestSecret)));

            var token = await sut.CreateTokenAsync(new CreateTokenRequest(1, "alice"));

            AssertSignedWith(token!, TestSecret);
        }

        [Fact]
        public async Task CreateTokenAsync_WhenIssuerAndAudienceNotConfigured_UsesDefaults()
        {
            var userManager = MockUserManager();
            userManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            var sut = new TokenService(userManager.Object, Config(("Jwt:Secret", TestSecret)));

            var token = await sut.CreateTokenAsync(new CreateTokenRequest(1, "alice"));

            var jwt = Decode(token!);
            Assert.Equal("BeStrong", jwt.Issuer);
            Assert.Equal("BeStrongUsers", jwt.Audiences.Single());
        }

        [Fact]
        public async Task CreateTokenAsync_WhenExpirationNotConfigured_DefaultsToSixtyMinutes()
        {
            var userManager = MockUserManager();
            userManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            var sut = new TokenService(userManager.Object, Config(("Jwt:Secret", TestSecret)));

            var before = DateTime.UtcNow;
            var token = await sut.CreateTokenAsync(new CreateTokenRequest(1, "alice"));
            var after = DateTime.UtcNow;

            var jwt = Decode(token!);
            Assert.InRange(jwt.ValidTo, before.AddMinutes(60).AddSeconds(-5), after.AddMinutes(60).AddSeconds(5));
        }

        [Fact]
        public async Task CreateTokenAsync_UsesConfiguredExpirationInMinutes()
        {
            var userManager = MockUserManager();
            userManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            var sut = new TokenService(userManager.Object, Config(("Jwt:Secret", TestSecret), ("Jwt:ExpirationInMinutes", "5")));

            var before = DateTime.UtcNow;
            var token = await sut.CreateTokenAsync(new CreateTokenRequest(1, "alice"));

            var jwt = Decode(token!);
            Assert.InRange(jwt.ValidTo, before.AddMinutes(5).AddSeconds(-5), before.AddMinutes(5).AddSeconds(5));
        }

        private static void AssertSignedWith(string token, string secret)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret))
            };

            var exception = Record.Exception(() => handler.ValidateToken(token, parameters, out _));
            Assert.Null(exception);
        }
    }
}
