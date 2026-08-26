using Application.Helpers.Criteria;
using Application.Mappings;
using Domain.Aggregates;
using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Searchers;
using Infrastructure.Tests.TestDoubles;

namespace Infrastructure.Tests.Searchers
{
    public class UserSearcherTests : SqliteInMemoryFixture
    {
        private readonly UserSearcher _sut;

        public UserSearcherTests()
        {
            _sut = new UserSearcher(Context);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsUsersOrderedByUserName()
        {
            await CreateUserAsync("bob");
            await CreateUserAsync("alice");

            var result = await _sut.GetAllAsync(CancellationToken.None);

            Assert.Equal(["alice", "bob"], result.Select(u => u.UserName));
        }

        [Fact]
        public async Task FindByUsernameAsync_IsCaseInsensitive()
        {
            await CreateUserAsync("Alice");

            var result = await _sut.FindByUsernameAsync("ALICE", CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindByUsernameAsync_WhenUserDoesNotExist_ReturnsNull()
        {
            var result = await _sut.FindByUsernameAsync("ghost", CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task FindByIdAsync_WhenUserDoesNotExist_ReturnsNull()
        {
            var result = await _sut.FindByIdAsync(999, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task FindByIdsAsync_ReturnsOnlyTheRequestedUsers()
        {
            var alice = await CreateUserAsync("alice");
            var bob = await CreateUserAsync("bob");
            await CreateUserAsync("carol");

            var result = await _sut.FindByIdsAsync([alice.Id, bob.Id], CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, u => u.UserName == "alice");
            Assert.Contains(result, u => u.UserName == "bob");
        }

        [Fact]
        public async Task ExistsAsync_WhenUserExists_ReturnsTrue()
        {
            var user = await CreateUserAsync();

            Assert.True(await _sut.ExistsAsync(user.Id, CancellationToken.None));
        }

        [Fact]
        public async Task ExistsAsync_WhenUserDoesNotExist_ReturnsFalse()
        {
            Assert.False(await _sut.ExistsAsync(999, CancellationToken.None));
        }

        [Fact]
        public async Task GetWorkoutStartDateAsync_ReturnsTheStoredDate()
        {
            var startDate = DateTime.UtcNow.AddMonths(-2);
            var user = new User { UserName = "alice", DateOfWorkoutStart = startDate };
            Context.Users.Add(user);
            await Context.SaveChangesAsync();

            var result = await _sut.GetWorkoutStartDateAsync(user.Id, CancellationToken.None);

            Assert.Equal(startDate, result);
        }

        [Fact]
        public async Task GetFollowedUserIdsAsync_ReturnsOnlyIdsFollowedByThatUser()
        {
            var user = await CreateUserAsync("alice");
            var followed1 = await CreateUserAsync("bob");
            var followed2 = await CreateUserAsync("carol");
            var stranger = await CreateUserAsync("dave");
            Context.Followers.AddRange(
                new Follower { UserId = user.Id, FollowedUserId = followed1.Id, FollowedAt = DateTime.UtcNow },
                new Follower { UserId = user.Id, FollowedUserId = followed2.Id, FollowedAt = DateTime.UtcNow },
                new Follower { UserId = stranger.Id, FollowedUserId = followed1.Id, FollowedAt = DateTime.UtcNow });
            await Context.SaveChangesAsync();

            var result = await _sut.GetFollowedUserIdsAsync(user.Id, CancellationToken.None);

            Assert.Equal([followed1.Id, followed2.Id], result.OrderBy(id => id));
        }

        [Fact]
        public async Task GetPagedAsync_WithoutUsernameFilter_ReturnsAllUsersPaged()
        {
            await CreateUserAsync("alice");
            await CreateUserAsync("bob");

            var page = await _sut.GetPagedAsync(new UserSearchCriteria { PageNumber = 1, PageSize = 10 }, CancellationToken.None);

            Assert.Equal(2, page.TotalItems);
            Assert.Equal(2, page.Count);
        }

        [Fact]
        public async Task GetPagedAsync_RespectsPageSize()
        {
            await CreateUserAsync("alice");
            await CreateUserAsync("bob");
            await CreateUserAsync("carol");

            var page = await _sut.GetPagedAsync(new UserSearchCriteria { PageNumber = 1, PageSize = 2 }, CancellationToken.None);

            Assert.Equal(3, page.TotalItems);
            Assert.Equal(2, page.Count);
            Assert.Equal(2, page.TotalPages);
        }

        [Fact]
        public async Task GetPagedAsync_WithExcludeUsernameCriteria_OmitsThatUserFromTheResults()
        {
            await CreateUserAsync("alice");
            await CreateUserAsync("bob");

            var page = await _sut.GetPagedAsync(new UserSearchCriteria { PageNumber = 1, PageSize = 10, ExcludeUsername = "alice" }, CancellationToken.None);

            Assert.DoesNotContain(page, u => u.UserName == "alice");
            Assert.Contains(page, u => u.UserName == "bob");
        }

        [Fact]
        public async Task GetPagedAsync_FiltersByUsernameSubstring_CaseInsensitive()
        {
            await CreateUserAsync("alice");
            await CreateUserAsync("bob");

            var page = await _sut.GetPagedAsync(new UserSearchCriteria { PageNumber = 1, PageSize = 10, Username = "ALI" }, CancellationToken.None);

            Assert.Single(page);
            Assert.Equal("alice", page[0].UserName);
        }

        [Fact]
        public async Task GetPagedAsync_FiltersByGender()
        {
            Context.Users.AddRange(
                new User { UserName = "alice", Gender = Gender.Female },
                new User { UserName = "bob", Gender = Gender.Male });
            await Context.SaveChangesAsync();

            var page = await _sut.GetPagedAsync(new UserSearchCriteria { PageNumber = 1, PageSize = 10, Gender = Gender.Female }, CancellationToken.None);

            Assert.Single(page);
            Assert.Equal("alice", page[0].UserName);
        }

        [Fact]
        public async Task GetPagedAsync_FiltersByCountrySubstring_CaseInsensitive()
        {
            Context.Users.AddRange(
                new User { UserName = "alice", Country = "Poland" },
                new User { UserName = "bob", Country = "France" });
            await Context.SaveChangesAsync();

            var page = await _sut.GetPagedAsync(new UserSearchCriteria { PageNumber = 1, PageSize = 10, Country = "poland" }, CancellationToken.None);

            Assert.Single(page);
            Assert.Equal("alice", page[0].UserName);
        }

        [Fact]
        public async Task GetPagedAsync_FiltersByCitySubstring_CaseInsensitive()
        {
            Context.Users.AddRange(
                new User { UserName = "alice", City = "Warsaw" },
                new User { UserName = "bob", City = "Paris" });
            await Context.SaveChangesAsync();

            var page = await _sut.GetPagedAsync(new UserSearchCriteria { PageNumber = 1, PageSize = 10, City = "warsaw" }, CancellationToken.None);

            Assert.Single(page);
            Assert.Equal("alice", page[0].UserName);
        }

        [Fact]
        public async Task FindByUsernameAsync_ComputesWorkoutSince()
        {
            var user = new User { UserName = "alice", NormalizedUserName = "ALICE", DateOfWorkoutStart = DateTime.UtcNow.AddDays(-40) };
            Context.Users.Add(user);
            await Context.SaveChangesAsync();

            var result = await _sut.FindByUsernameAsync("alice", CancellationToken.None);

            Assert.NotNull(result!.WorkoutSince);
        }

        [Fact]
        public async Task GetAllAsync_DoesNotComputeWorkoutSince_UnlikeFindByUsernameAsync()
        {
            // UserMappings.Selector always sets WorkoutSince to null; only FindByUsernameAsync and
            // FindByIdAsync call .WithComputedWorkoutSince() afterwards. GetAllAsync, FindByIdsAsync,
            // and GetPagedAsync don't, so WorkoutSince silently comes back null from those three even
            // when DateOfWorkoutStart is set — an inconsistency worth knowing about before relying on
            // WorkoutSince from a list endpoint.
            Context.Users.Add(new User { UserName = "alice", DateOfWorkoutStart = DateTime.UtcNow.AddDays(-40) });
            await Context.SaveChangesAsync();

            var result = await _sut.GetAllAsync(CancellationToken.None);

            Assert.Null(result.Single().WorkoutSince);
        }

        [Fact]
        public async Task GetSettingsAsync_WhenUserHasNoSettings_ReturnsDefault()
        {
            var user = await CreateUserAsync();

            var result = await _sut.GetSettingsAsync(user.Id, CancellationToken.None);

            Assert.Equal(UserSettingsMappings.Default, result);
        }

        [Fact]
        public async Task GetSettingsAsync_WhenUserHasSettings_ReturnsThem()
        {
            var user = new User
            {
                UserName = "alice",
                Settings = new UserSettings(
                    ProfileVisibility.Private, ProfileVisibility.FollowersOnly, ProfileVisibility.Private, ProfileVisibility.Public,
                    autoPublishWorkouts: false, autoPublishWorkoutPlanChanges: false)
            };
            Context.Users.Add(user);
            await Context.SaveChangesAsync();

            var result = await _sut.GetSettingsAsync(user.Id, CancellationToken.None);

            Assert.Equal(ProfileVisibility.Private, result.PhotosVisibility);
            Assert.Equal(ProfileVisibility.FollowersOnly, result.WorkoutsVisibility);
            Assert.Equal(ProfileVisibility.Private, result.WorkoutPlanVisibility);
            Assert.Equal(ProfileVisibility.Public, result.MeasurementsVisibility);
            Assert.False(result.AutoPublishWorkouts);
            Assert.False(result.AutoPublishWorkoutPlanChanges);
        }
    }
}
