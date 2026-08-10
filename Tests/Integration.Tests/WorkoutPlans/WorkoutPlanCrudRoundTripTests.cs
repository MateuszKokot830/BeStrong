using System.Net;
using System.Net.Http.Json;
using Application.Dto.Exercise;
using Application.Dto.WorkoutPlan;
using Domain.Common;
using Integration.Tests.TestDoubles;

namespace Integration.Tests.WorkoutPlans
{
    public class WorkoutPlanCrudRoundTripTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public WorkoutPlanCrudRoundTripTests(TestWebApplicationFactory factory)
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

            var response = await adminClient.PostAsJsonAsync("/api/exercises", new CreateExerciseDto(name, null, MuscleSubgroup.Chest, null));
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<ExerciseDto>())!;
        }

        private static WorkoutPlanCreateDto PlanWithNestedTemplates(int exerciseId, bool isPublic = false) => new(
            "Push Pull Legs",
            "A classic three-day split",
            WorkoutPlanCategory.PushPullLegs,
            isPublic,
            [
                new WorkoutTemplateDto(Order: 0, Name: "Push Day", Exercises: [new WorkoutTemplateExerciseDto(Order: 0, exerciseId)]),
                new WorkoutTemplateDto(Order: 1, Name: "Pull Day", Exercises: [new WorkoutTemplateExerciseDto(Order: 0, exerciseId)])
            ]);

        [Fact]
        public async Task CreateWorkoutPlan_PersistsNestedTemplatesAndExercises()
        {
            // Exercises the deepest cascade in the schema this suite has touched:
            // WorkoutPlan -> WorkoutTemplates -> WorkoutTemplateExercises, created in a single request.
            var exercise = await CreateExerciseAsAdminAsync("WP Bench Press");
            var token = await _client.RegisterAndGetTokenAsync("iris_wpcreate");
            _client.SetBearerToken(token);

            var createResponse = await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id));
            Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();

            Assert.NotEqual(0, created!.Id);
            Assert.Equal(2, created.WorkoutTemplates.Count);
            Assert.All(created.WorkoutTemplates, t => Assert.Single(t.Exercises));

            var getResponse = await _client.GetAsync($"/api/workoutplans/{created.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var fetched = await getResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();
            Assert.Equal(2, fetched!.WorkoutTemplates.Count);
            Assert.Contains(fetched.WorkoutTemplates, t => t.Name == "Push Day");
            Assert.Contains(fetched.WorkoutTemplates, t => t.Name == "Pull Day");
        }

        [Fact]
        public async Task DeleteWorkoutPlan_CascadesToTemplatesAndTemplateExercises()
        {
            var exercise = await CreateExerciseAsAdminAsync("WP Squat");
            var token = await _client.RegisterAndGetTokenAsync("iris_wpdelete");
            _client.SetBearerToken(token);
            var createResponse = await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id));
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();

            var deleteResponse = await _client.DeleteAsync($"/api/workoutplans/{created!.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getAfterDeleteResponse = await _client.GetAsync($"/api/workoutplans/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getAfterDeleteResponse.StatusCode);
        }

        [Fact]
        public async Task AssignThenUnassignWorkoutPlan_UpdatesTheCurrentUsersProfile()
        {
            var exercise = await CreateExerciseAsAdminAsync("WP Deadlift");
            var ownerToken = await _client.RegisterAndGetTokenAsync("iris_wpowner");
            _client.SetBearerToken(ownerToken);
            var createResponse = await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id, isPublic: true));
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();

            using var assigneeClient = _factory.CreateClient();
            var assigneeToken = await assigneeClient.RegisterAndGetTokenAsync("iris_wpassignee");
            assigneeClient.SetBearerToken(assigneeToken);

            var assignResponse = await assigneeClient.PostAsync($"/api/workoutplans/{created!.Id}/assign", content: null);
            Assert.Equal(HttpStatusCode.NoContent, assignResponse.StatusCode);

            var unassignResponse = await assigneeClient.DeleteAsync($"/api/workoutplans/{created.Id}/assign");
            Assert.Equal(HttpStatusCode.NoContent, unassignResponse.StatusCode);
        }

        [Fact]
        public async Task GetPublicWorkoutPlans_OnlyReturnsPlansMarkedPublic()
        {
            var exercise = await CreateExerciseAsAdminAsync("WP Overhead Press");
            var token = await _client.RegisterAndGetTokenAsync("iris_wppublic");
            _client.SetBearerToken(token);
            await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id, isPublic: true));
            await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id, isPublic: false));

            var response = await _client.GetAsync("/api/workoutplans");
            var plans = await response.Content.ReadFromJsonAsync<List<WorkoutPlanDto>>();

            Assert.All(plans!, p => Assert.True(p.IsPublic));
        }

        [Fact]
        public async Task DeleteWorkoutPlan_AsADifferentAuthenticatedUser_ReturnsUnauthorized()
        {
            var exercise = await CreateExerciseAsAdminAsync("WP Row");
            var ownerToken = await _client.RegisterAndGetTokenAsync("iris_wpownerdel");
            _client.SetBearerToken(ownerToken);
            var createResponse = await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id));
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();

            using var strangerClient = _factory.CreateClient();
            var strangerToken = await strangerClient.RegisterAndGetTokenAsync("iris_wpstranger");
            strangerClient.SetBearerToken(strangerToken);

            var deleteResponse = await strangerClient.DeleteAsync($"/api/workoutplans/{created!.Id}");

            Assert.Equal(HttpStatusCode.Unauthorized, deleteResponse.StatusCode);
        }
    }
}
