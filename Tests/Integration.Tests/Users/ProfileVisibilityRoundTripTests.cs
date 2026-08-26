using System.Net;
using System.Net.Http.Json;
using Application.Dto.User;
using Application.Dto.Workout;
using Domain.Common;
using Integration.Tests.TestDoubles;

namespace Integration.Tests.Users
{
    public class ProfileVisibilityRoundTripTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public ProfileVisibilityRoundTripTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private async Task<(HttpClient Client, int UserId, string Username)> RegisterClientAsync(string username)
        {
            var client = _factory.CreateClient();
            var token = await client.RegisterAndGetTokenAsync(username);
            client.SetBearerToken(token);
            var me = await client.GetFromJsonAsync<UserDto>("/api/users/me");
            return (client, me!.Id, username);
        }

        private static UserSettingsDto Settings(
            ProfileVisibility photos = ProfileVisibility.Public,
            ProfileVisibility workouts = ProfileVisibility.Public,
            ProfileVisibility workoutPlan = ProfileVisibility.Public,
            ProfileVisibility measurements = ProfileVisibility.Public,
            bool autoPublishWorkouts = true,
            bool autoPublishWorkoutPlanChanges = true) =>
            new(photos, workouts, workoutPlan, measurements, autoPublishWorkouts, autoPublishWorkoutPlanChanges);

        [Fact]
        public async Task GetUserSettings_BeforeAnyUpdate_ReturnsPublicDefaults()
        {
            var (client, _, _) = await RegisterClientAsync("iris_settingsdefault");

            var response = await client.GetAsync("/api/users/settings");

            response.EnsureSuccessStatusCode();
            var settings = await response.Content.ReadFromJsonAsync<UserSettingsDto>();
            Assert.Equal(ProfileVisibility.Public, settings!.PhotosVisibility);
            Assert.Equal(ProfileVisibility.Public, settings.WorkoutsVisibility);
            Assert.Equal(ProfileVisibility.Public, settings.WorkoutPlanVisibility);
            Assert.Equal(ProfileVisibility.Public, settings.MeasurementsVisibility);
            Assert.True(settings.AutoPublishWorkouts);
            Assert.True(settings.AutoPublishWorkoutPlanChanges);
        }

        [Fact]
        public async Task UpdateUserSettings_ThenGet_PersistsTheChange()
        {
            var (client, _, _) = await RegisterClientAsync("iris_settingsupdate");

            var putResponse = await client.PutAsJsonAsync("/api/users/settings", Settings(photos: ProfileVisibility.Private, autoPublishWorkouts: false));
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

            var getResponse = await client.GetAsync("/api/users/settings");
            var settings = await getResponse.Content.ReadFromJsonAsync<UserSettingsDto>();
            Assert.Equal(ProfileVisibility.Private, settings!.PhotosVisibility);
            Assert.False(settings.AutoPublishWorkouts);
        }

        [Fact]
        public async Task GetUser_AsAStranger_WhenPhotosArePrivate_HidesPhotosAndReportsCannotView()
        {
            var (owner, _, ownerUsername) = await RegisterClientAsync("iris_photoowner");
            await owner.PutAsJsonAsync("/api/users/settings", Settings(photos: ProfileVisibility.Private));
            var (stranger, _, _) = await RegisterClientAsync("iris_photostranger");

            var response = await stranger.GetAsync($"/api/users/{ownerUsername}");

            response.EnsureSuccessStatusCode();
            var profile = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.False(profile!.CanViewPhotos);
            Assert.Empty(profile.Photos);
        }

        [Fact]
        public async Task GetUser_AsTheOwner_AlwaysSeesEverythingRegardlessOfOwnSettings()
        {
            var (owner, _, ownerUsername) = await RegisterClientAsync("iris_selfowner");
            await owner.PutAsJsonAsync("/api/users/settings", Settings(
                photos: ProfileVisibility.Private, workouts: ProfileVisibility.Private,
                workoutPlan: ProfileVisibility.Private, measurements: ProfileVisibility.Private));

            var response = await owner.GetAsync($"/api/users/{ownerUsername}");

            response.EnsureSuccessStatusCode();
            var profile = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.True(profile!.CanViewPhotos);
            Assert.True(profile.CanViewWorkouts);
            Assert.True(profile.CanViewWorkoutPlan);
            Assert.True(profile.CanViewMeasurements);
        }

        [Fact]
        public async Task GetUser_AsAFollower_WhenFollowersOnly_CanView()
        {
            var (owner, ownerId, ownerUsername) = await RegisterClientAsync("iris_folowner");
            await owner.PutAsJsonAsync("/api/users/settings", Settings(measurements: ProfileVisibility.FollowersOnly));
            var (follower, followerId, _) = await RegisterClientAsync("iris_folfollower");
            await follower.PostAsync($"/api/users/{followerId}/follow?followUserId={ownerId}", content: null);

            var response = await follower.GetAsync($"/api/users/{ownerUsername}");

            response.EnsureSuccessStatusCode();
            var profile = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.True(profile!.CanViewMeasurements);
        }

        [Fact]
        public async Task GetUserWorkouts_AsAStranger_WhenWorkoutsArePrivate_ReturnsEmptyListNotAnError()
        {
            var (owner, ownerId, _) = await RegisterClientAsync("iris_workoutowner");
            await owner.PutAsJsonAsync("/api/users/settings", Settings(workouts: ProfileVisibility.Private));
            var (stranger, _, _) = await RegisterClientAsync("iris_workoutstranger");

            var response = await stranger.GetAsync($"/api/workouts/{ownerId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var workouts = await response.Content.ReadFromJsonAsync<List<WorkoutDto>>();
            Assert.Empty(workouts!);
        }
    }
}
