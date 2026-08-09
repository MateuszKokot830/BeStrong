using System.Security.Claims;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Infrastructure.Tests.Services
{
    public class CurrentUserServiceTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();

        private CurrentUserService CreateSut() => new(_httpContextAccessor.Object);

        private void SetUser(params Claim[] claims)
        {
            var identity = new ClaimsIdentity(claims, authenticationType: "Test");
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            });
        }

        [Fact]
        public void UserId_WithValidNameIdentifierClaim_ReturnsParsedId()
        {
            SetUser(new Claim(ClaimTypes.NameIdentifier, "42"));

            Assert.Equal(42, CreateSut().UserId);
        }

        [Fact]
        public void UserId_WhenHttpContextIsNull_ReturnsZero()
        {
            _httpContextAccessor.Setup(a => a.HttpContext).Returns((HttpContext?)null);

            Assert.Equal(0, CreateSut().UserId);
        }

        [Fact]
        public void UserId_WhenNameIdentifierClaimIsMissing_ReturnsZero()
        {
            SetUser();

            Assert.Equal(0, CreateSut().UserId);
        }

        [Fact]
        public void UserId_WhenNameIdentifierClaimIsNotNumeric_ReturnsZero()
        {
            SetUser(new Claim(ClaimTypes.NameIdentifier, "not-a-number"));

            Assert.Equal(0, CreateSut().UserId);
        }

        [Fact]
        public void IsAdmin_WithAdminRoleClaim_ReturnsTrue()
        {
            SetUser(new Claim(ClaimTypes.Role, "Admin"));

            Assert.True(CreateSut().IsAdmin);
        }

        [Fact]
        public void IsAdmin_WithoutAdminRoleClaim_ReturnsFalse()
        {
            SetUser(new Claim(ClaimTypes.Role, "Member"));

            Assert.False(CreateSut().IsAdmin);
        }

        [Fact]
        public void IsAdmin_WhenHttpContextIsNull_ReturnsFalse()
        {
            _httpContextAccessor.Setup(a => a.HttpContext).Returns((HttpContext?)null);

            Assert.False(CreateSut().IsAdmin);
        }

        [Fact]
        public void IsOwnerOrAdmin_WhenCallerIsTheOwner_ReturnsTrue()
        {
            SetUser(new Claim(ClaimTypes.NameIdentifier, "5"));

            Assert.True(CreateSut().IsOwnerOrAdmin(5));
        }

        [Fact]
        public void IsOwnerOrAdmin_WhenCallerIsAdminButNotOwner_ReturnsTrue()
        {
            SetUser(new Claim(ClaimTypes.NameIdentifier, "5"), new Claim(ClaimTypes.Role, "Admin"));

            Assert.True(CreateSut().IsOwnerOrAdmin(999));
        }

        [Fact]
        public void IsOwnerOrAdmin_WhenCallerIsNeitherOwnerNorAdmin_ReturnsFalse()
        {
            SetUser(new Claim(ClaimTypes.NameIdentifier, "5"));

            Assert.False(CreateSut().IsOwnerOrAdmin(999));
        }
    }
}
