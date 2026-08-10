using System.Net.Http.Json;
using Application.Dto.Exercise;
using Domain.Common;
using Integration.Tests.TestDoubles;

namespace Integration.Tests.Exercises
{
    public class ExerciseCacheInvalidationTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ExerciseCacheInvalidationTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private async Task AuthenticateAsAdminAsync()
        {
            var token = await _client.LoginAndGetTokenAsync(
                AuthenticatedClientExtensions.SeedAdminUsername, AuthenticatedClientExtensions.SeedAdminPassword);
            _client.SetBearerToken(token);
        }

        [Fact]
        public async Task CreateExercise_InvalidatesTheCachedExerciseList()
        {
            // ExercisesController.GetExercises is backed by CachedExerciseSearcher, which caches
            // GetAllAsync's result under CachedExerciseSearcher.CacheKey and relies on
            // ExerciseChangedNotificationHandler to evict that entry whenever an exercise changes.
            // Nothing tests these two pieces working together anywhere else — a unit test mocking
            // IMemoryCache can prove the eviction call happens, but not that the real cached HTTP
            // response actually goes stale-then-fresh across two real requests.
            await AuthenticateAsAdminAsync();

            var before = await _client.GetFromJsonAsync<List<ExerciseDto>>("/api/exercises");
            var beforeCount = before!.Count;

            var createResponse = await _client.PostAsJsonAsync("/api/exercises",
                new CreateExerciseDto("Integration Test Curl", null, MuscleSubgroup.Biceps, null));
            createResponse.EnsureSuccessStatusCode();

            var after = await _client.GetFromJsonAsync<List<ExerciseDto>>("/api/exercises");

            Assert.Equal(beforeCount + 1, after!.Count);
            Assert.Contains(after, e => e.Name == "Integration Test Curl");
        }

        [Fact]
        public async Task DeleteExercise_InvalidatesTheCachedExerciseList()
        {
            await AuthenticateAsAdminAsync();

            var createResponse = await _client.PostAsJsonAsync("/api/exercises",
                new CreateExerciseDto("Integration Test Curl To Delete", null, MuscleSubgroup.Biceps, null));
            var created = await createResponse.Content.ReadFromJsonAsync<ExerciseDto>();

            // Prime the cache with the exercise present, then delete it and confirm the very next
            // read no longer sees it — proving eviction, not coincidental cache expiry.
            await _client.GetFromJsonAsync<List<ExerciseDto>>("/api/exercises");
            var deleteResponse = await _client.DeleteAsync($"/api/exercises/{created!.Id}");
            deleteResponse.EnsureSuccessStatusCode();

            var after = await _client.GetFromJsonAsync<List<ExerciseDto>>("/api/exercises");

            Assert.DoesNotContain(after!, e => e.Id == created.Id);
        }

        [Fact]
        public async Task UpdateExercise_InvalidatesTheCachedExerciseList()
        {
            await AuthenticateAsAdminAsync();

            var createResponse = await _client.PostAsJsonAsync("/api/exercises",
                new CreateExerciseDto("Integration Test Curl To Update", null, MuscleSubgroup.Biceps, null));
            var created = await createResponse.Content.ReadFromJsonAsync<ExerciseDto>();

            await _client.GetFromJsonAsync<List<ExerciseDto>>("/api/exercises");
            var updateResponse = await _client.PutAsJsonAsync($"/api/exercises/{created!.Id}",
                new CreateExerciseDto("Renamed Integration Test Curl", null, MuscleSubgroup.Biceps, null));
            updateResponse.EnsureSuccessStatusCode();

            var after = await _client.GetFromJsonAsync<List<ExerciseDto>>("/api/exercises");

            Assert.Contains(after!, e => e.Id == created.Id && e.Name == "Renamed Integration Test Curl");
        }
    }
}
