using System.Net;
using System.Net.Http.Json;
using Application.Dto.Exercise;
using Application.Dto.Post;
using Application.Dto.User;
using Application.Dto.Workout;
using Domain.Common;
using Integration.Tests.TestDoubles;

namespace Integration.Tests.Workouts
{
    public class WorkoutCrudRoundTripTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public WorkoutCrudRoundTripTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private async Task<ExerciseDto> CreateExerciseAsAdminAsync(string name)
        {
            using var adminClient = _factory.CreateClient();
            var adminToken = await adminClient.LoginAndGetTokenAsync(
                AuthenticatedClientExtensions.SeedAdminUsername, AuthenticatedClientExtensions.SeedAdminPassword);
            adminClient.SetBearerToken(adminToken);

            var response = await adminClient.PostAsJsonAsync("/api/exercises", new CreateExerciseDto(name, null, MuscleSubgroup.Quads, null));
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<ExerciseDto>())!;
        }

        private static CreateWorkoutDto WorkoutWithOneExercise(int exerciseId, string name = "Leg Day") => new(
            name,
            [new WorkoutExerciseDto(Order: 0, Notes: null, exerciseId, WorkoutId: 0, null, null,
                [new WorkoutSetDto(SetNumber: 1, Reps: 10, Weight: 100, null, null)])]);

        [Fact]
        public async Task CreateGetUpdateDelete_RoundTripsThroughTheRealDatabase()
        {
            var exercise = await CreateExerciseAsAdminAsync("Squat");
            var token = await _client.RegisterAndGetTokenAsync("jack_workoutcrud");
            _client.SetBearerToken(token);
            var me = await _client.GetFromJsonAsync<UserDto>("/api/users/me");

            var createResponse = await _client.PostAsJsonAsync("/api/workouts", WorkoutWithOneExercise(exercise.Id));
            Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutDto>();
            Assert.NotEqual(0, created!.Id);
            Assert.Single(created.WorkoutExercises);
            Assert.Single(created.WorkoutExercises.First().Sets);

            var listResponse = await _client.GetAsync($"/api/workouts/{me!.Id}");
            Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);
            var workouts = await listResponse.Content.ReadFromJsonAsync<List<WorkoutDto>>();
            Assert.Contains(workouts!, w => w.Id == created.Id && w.Name == "Leg Day");

            var updateResponse = await _client.PutAsJsonAsync($"/api/workouts/{created.Id}", WorkoutWithOneExercise(exercise.Id, "Renamed Leg Day"));
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
            var updated = await updateResponse.Content.ReadFromJsonAsync<WorkoutDto>();
            Assert.Equal("Renamed Leg Day", updated!.Name);

            var deleteResponse = await _client.DeleteAsync($"/api/workouts/{created.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var listAfterDeleteResponse = await _client.GetAsync($"/api/workouts/{me.Id}");
            var workoutsAfterDelete = await listAfterDeleteResponse.Content.ReadFromJsonAsync<List<WorkoutDto>>();
            Assert.DoesNotContain(workoutsAfterDelete!, w => w.Id == created.Id);
        }

        [Fact]
        public async Task CreateWorkout_AutomaticallyPublishesAWorkoutPublicationPost()
        {
            var exercise = await CreateExerciseAsAdminAsync("Overhead Press");
            var token = await _client.RegisterAndGetTokenAsync("jack_workoutpost");
            _client.SetBearerToken(token);

            var createResponse = await _client.PostAsJsonAsync("/api/workouts", WorkoutWithOneExercise(exercise.Id, "Shoulder Day"));
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutDto>();

            var postsResponse = await _client.GetAsync("/api/posts");
            var posts = await postsResponse.Content.ReadFromJsonAsync<List<PostDto>>();

            var post = Assert.Single(posts!, p => p.WorkoutId == created!.Id);
            Assert.Equal(PostType.WorkoutPublication, post.Type);
            Assert.Equal("Shoulder Day", post.Description);
            Assert.NotNull(post.Workout);
            Assert.Single(post.Workout!.WorkoutExercises);
            Assert.Single(post.Workout.WorkoutExercises.First().Sets);
        }

        [Fact]
        public async Task DeleteWorkout_AlsoRemovesItsWorkoutPublicationPost()
        {
            var exercise = await CreateExerciseAsAdminAsync("Lat Pulldown");
            var token = await _client.RegisterAndGetTokenAsync("jack_workoutpostdel");
            _client.SetBearerToken(token);
            var createResponse = await _client.PostAsJsonAsync("/api/workouts", WorkoutWithOneExercise(exercise.Id, "Back Day"));
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutDto>();

            var postsBeforeResponse = await _client.GetAsync("/api/posts");
            var postsBefore = await postsBeforeResponse.Content.ReadFromJsonAsync<List<PostDto>>();
            var post = postsBefore!.Single(p => p.WorkoutId == created!.Id);

            var deleteResponse = await _client.DeleteAsync($"/api/workouts/{created!.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getPostResponse = await _client.GetAsync($"/api/posts/{post.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getPostResponse.StatusCode);
        }

        [Fact]
        public async Task GetUserWorkouts_AsADifferentAuthenticatedUser_ReturnsUnauthorized()
        {
            var token = await _client.RegisterAndGetTokenAsync("jack_workoutowner");
            _client.SetBearerToken(token);
            var me = await _client.GetFromJsonAsync<UserDto>("/api/users/me");

            using var strangerClient = _factory.CreateClient();
            var strangerToken = await strangerClient.RegisterAndGetTokenAsync("jack_workoutstranger");
            strangerClient.SetBearerToken(strangerToken);

            var response = await strangerClient.GetAsync($"/api/workouts/{me!.Id}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DeleteWorkout_AsADifferentAuthenticatedUser_ReturnsUnauthorized()
        {
            var exercise = await CreateExerciseAsAdminAsync("Deadlift");
            var ownerToken = await _client.RegisterAndGetTokenAsync("jack_workoutdelowner");
            _client.SetBearerToken(ownerToken);
            var createResponse = await _client.PostAsJsonAsync("/api/workouts", WorkoutWithOneExercise(exercise.Id));
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutDto>();

            using var strangerClient = _factory.CreateClient();
            var strangerToken = await strangerClient.RegisterAndGetTokenAsync("jack_workoutdelstranger");
            strangerClient.SetBearerToken(strangerToken);

            var deleteResponse = await strangerClient.DeleteAsync($"/api/workouts/{created!.Id}");

            Assert.Equal(HttpStatusCode.Unauthorized, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task CalculateOneRepMax_ReturnsTheComputedValue()
        {
            var token = await _client.RegisterAndGetTokenAsync("jack_onerepmax");
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/api/workouts/weight/100/reps/5");

            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<int>();
            Assert.Equal(113, value);
        }

        [Fact]
        public async Task GetWorkoutStatistics_AfterLoggingAWorkout_ReflectsIt()
        {
            var exercise = await CreateExerciseAsAdminAsync("Bench Press For Stats");
            var token = await _client.RegisterAndGetTokenAsync("jack_stats");
            _client.SetBearerToken(token);
            var me = await _client.GetFromJsonAsync<UserDto>("/api/users/me");
            await _client.PostAsJsonAsync("/api/workouts", WorkoutWithOneExercise(exercise.Id));

            var response = await _client.GetAsync($"/api/workouts/statistics/{me!.Id}");

            response.EnsureSuccessStatusCode();
            var stats = await response.Content.ReadFromJsonAsync<Application.Dto.Statistics.StatisticsDto>();
            Assert.Equal(1, stats!.TotalWorkouts);
            Assert.Equal(1, stats.TotalSets);
        }
    }
}
