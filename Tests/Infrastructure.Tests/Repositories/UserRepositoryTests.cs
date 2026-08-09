using Domain.Aggregates;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Tests.TestDoubles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Infrastructure.Tests.Repositories
{
    public class UserRepositoryTests : SqliteInMemoryFixture
    {
        private readonly UserManager<User> _userManager;
        private readonly UserRepository _sut;

        public UserRepositoryTests()
        {
            var store = new UserStore<User, Role, DataContext, int>(Context);
            var options = Options.Create(new IdentityOptions
            {
                Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequiredLength = 1,
                    RequireLowercase = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                }
            });

            _userManager = new UserManager<User>(
                store,
                options,
                new PasswordHasher<User>(),
                [],
                [],
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                null!,
                NullLogger<UserManager<User>>.Instance);

            _sut = new UserRepository(Context, _userManager);
        }

        [Fact]
        public async Task RegisterUserAsync_WithValidPassword_CreatesTheUser()
        {
            var user = new User { UserName = "alice" };

            var result = await _sut.RegisterUserAsync(user, "password", CancellationToken.None);

            Assert.True(result.Succeeded);
            Assert.NotEqual(0, user.Id);
        }

        [Fact]
        public async Task GetByUsernameAsync_IsCaseInsensitive()
        {
            await _sut.RegisterUserAsync(new User { UserName = "Alice" }, "password", CancellationToken.None);

            var found = await _sut.GetByUsernameAsync("aLICE", CancellationToken.None);

            Assert.NotNull(found);
            Assert.Equal("Alice", found!.UserName);
        }

        [Fact]
        public async Task GetByUsernameAsync_WhenUserDoesNotExist_ReturnsNull()
        {
            var found = await _sut.GetByUsernameAsync("ghost", CancellationToken.None);

            Assert.Null(found);
        }

        [Fact]
        public async Task GetByUsernameAsync_WhenUsernameIsNull_ReturnsNullWithoutThrowing()
        {
            var found = await _sut.GetByUsernameAsync(null, CancellationToken.None);

            Assert.Null(found);
        }

        [Fact]
        public async Task CheckPasswordAsync_WithCorrectPassword_ReturnsTrue()
        {
            var user = new User { UserName = "alice" };
            await _sut.RegisterUserAsync(user, "password", CancellationToken.None);

            var result = await _sut.CheckPasswordAsync(user, "password", CancellationToken.None);

            Assert.True(result);
        }

        [Fact]
        public async Task CheckPasswordAsync_WithWrongPassword_ReturnsFalse()
        {
            var user = new User { UserName = "alice" };
            await _sut.RegisterUserAsync(user, "password", CancellationToken.None);

            var result = await _sut.CheckPasswordAsync(user, "wrong-password", CancellationToken.None);

            Assert.False(result);
        }

        [Fact]
        public async Task AddFollowerAsync_ThenSaveChanges_PersistsTheFollowerRelationship()
        {
            var user = await CreateUserAsync("alice");
            var followedUser = await CreateUserAsync("bob");
            var follower = new Follower { UserId = user.Id, FollowedUserId = followedUser.Id, FollowedAt = DateTime.UtcNow };

            await _sut.AddFollowerAsync(follower, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var reloaded = await _sut.GetByIdAsync(user.Id, CancellationToken.None);
            Assert.Single(reloaded!.FollowedUsers);
        }

        [Fact]
        public async Task DeleteFollowerAsync_ThenSaveChanges_RemovesTheFollowerRelationship()
        {
            var user = await CreateUserAsync("alice");
            var followedUser = await CreateUserAsync("bob");
            var follower = new Follower { UserId = user.Id, FollowedUserId = followedUser.Id, FollowedAt = DateTime.UtcNow };
            await _sut.AddFollowerAsync(follower, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var reloadedUser = await _sut.GetByIdAsync(user.Id, CancellationToken.None);
            await _sut.DeleteFollowerAsync(reloadedUser!.FollowedUsers.Single(), CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var finalUser = await _sut.GetByIdAsync(user.Id, CancellationToken.None);
            Assert.Empty(finalUser!.FollowedUsers);
        }

        [Fact]
        public async Task AddPhotoAsync_ThenSaveChanges_PersistsThePhoto()
        {
            var user = await CreateUserAsync("alice");
            var photo = new Photo { UserId = user.Id, Url = "http://img/1.jpg", IsProfilePhoto = true };

            await _sut.AddPhotoAsync(photo, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var reloaded = await _sut.GetByIdAsync(user.Id, CancellationToken.None);
            Assert.Single(reloaded!.Photos);
        }

        [Fact]
        public async Task DeletePhotoAsync_ThenSaveChanges_RemovesThePhoto()
        {
            var user = await CreateUserAsync("alice");
            var photo = new Photo { UserId = user.Id, Url = "http://img/1.jpg", IsProfilePhoto = true };
            await _sut.AddPhotoAsync(photo, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var reloadedUser = await _sut.GetByIdAsync(user.Id, CancellationToken.None);
            await _sut.DeletePhotoAsync(reloadedUser!.Photos.Single(), CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var finalUser = await _sut.GetByIdAsync(user.Id, CancellationToken.None);
            Assert.Empty(finalUser!.Photos);
        }
    }
}
