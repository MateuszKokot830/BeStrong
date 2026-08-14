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
                new WorkoutTemplateCreateDto(Order: 0, Name: "Push Day", Exercises: [new WorkoutTemplateExerciseCreateDto(Order: 0, exerciseId, Sets: 3, MinReps: 8, MaxReps: 10)]),
                new WorkoutTemplateCreateDto(Order: 1, Name: "Pull Day", Exercises: [new WorkoutTemplateExerciseCreateDto(Order: 0, exerciseId, Sets: 4, MinReps: 6, MaxReps: 8)])
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
            Assert.Equal("Push Pull Legs", created.CategoryLabel);

            var pushDay = created.WorkoutTemplates.Single(t => t.Name == "Push Day");
            var pushExercise = pushDay.Exercises.Single();
            Assert.Equal(exercise.Id, pushExercise.Exercise.Id);
            Assert.Equal(exercise.Name, pushExercise.Exercise.Name);
            Assert.Equal(3, pushExercise.Sets);
            Assert.Equal(8, pushExercise.MinReps);
            Assert.Equal(10, pushExercise.MaxReps);

            var getResponse = await _client.GetAsync($"/api/workoutplans/{created.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var fetched = await getResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();
            Assert.Equal(2, fetched!.WorkoutTemplates.Count);
            Assert.Contains(fetched.WorkoutTemplates, t => t.Name == "Push Day");
            Assert.Contains(fetched.WorkoutTemplates, t => t.Name == "Pull Day");
            Assert.Contains(fetched.WorkoutTemplates, t => t.Exercises.Single().Exercise.Id == exercise.Id);
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
        public async Task GetWorkoutPlans_ReturnsPublicAndOwnPlans_ExcludesOthersPrivatePlans()
        {
            var exercise = await CreateExerciseAsAdminAsync("WP Overhead Press");
            var token = await _client.RegisterAndGetTokenAsync("iris_wpbrowse");
            _client.SetBearerToken(token);
            var publicResponse = await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id, isPublic: true));
            var publicPlan = await publicResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();
            var privateResponse = await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id, isPublic: false));
            var privatePlan = await privateResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();

            using var strangerClient = _factory.CreateClient();
            var strangerToken = await strangerClient.RegisterAndGetTokenAsync("iris_wpbrowsestranger");
            strangerClient.SetBearerToken(strangerToken);
            var strangerResponse = await strangerClient.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id, isPublic: false));
            var strangerPrivatePlan = await strangerResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();

            var response = await _client.GetAsync("/api/workoutplans");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Headers.Contains("Pagination"));
            var plans = await response.Content.ReadFromJsonAsync<List<WorkoutPlanDto>>();

            Assert.Contains(plans!, p => p.Id == publicPlan!.Id);
            Assert.Contains(plans!, p => p.Id == privatePlan!.Id);
            Assert.DoesNotContain(plans!, p => p.Id == strangerPrivatePlan!.Id);
        }

        [Fact]
        public async Task GetWorkoutPlans_FiltersByCategoryAndName()
        {
            var exercise = await CreateExerciseAsAdminAsync("WP Lateral Raise");
            var token = await _client.RegisterAndGetTokenAsync("iris_wpfilter");
            _client.SetBearerToken(token);
            var createResponse = await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id, isPublic: true));
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();

            var matchResponse = await _client.GetAsync("/api/workoutplans?category=1&name=Push%20Pull%20Legs");
            var matchPlans = await matchResponse.Content.ReadFromJsonAsync<List<WorkoutPlanDto>>();
            Assert.Contains(matchPlans!, p => p.Id == created!.Id);

            var noMatchResponse = await _client.GetAsync("/api/workoutplans?category=0");
            var noMatchPlans = await noMatchResponse.Content.ReadFromJsonAsync<List<WorkoutPlanDto>>();
            Assert.DoesNotContain(noMatchPlans!, p => p.Id == created!.Id);
        }

        [Fact]
        public async Task UpdateWorkoutPlan_WhenNotInUse_PersistsChanges()
        {
            var exercise = await CreateExerciseAsAdminAsync("WP Leg Curl");
            var token = await _client.RegisterAndGetTokenAsync("iris_wpupdate");
            _client.SetBearerToken(token);
            var createResponse = await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id));
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();

            var updateDto = PlanWithNestedTemplates(exercise.Id) with { Name = "Renamed Plan" };
            var updateResponse = await _client.PutAsJsonAsync($"/api/workoutplans/{created!.Id}", updateDto);
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
            var updated = await updateResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();
            Assert.Equal("Renamed Plan", updated!.Name);

            var getResponse = await _client.GetAsync($"/api/workoutplans/{created.Id}");
            var fetched = await getResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();
            Assert.Equal("Renamed Plan", fetched!.Name);
        }

        [Fact]
        public async Task UpdateWorkoutPlan_AsADifferentUser_ReturnsUnauthorized()
        {
            var exercise = await CreateExerciseAsAdminAsync("WP Face Pull");
            var ownerToken = await _client.RegisterAndGetTokenAsync("iris_wpupdateowner");
            _client.SetBearerToken(ownerToken);
            var createResponse = await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id));
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();

            using var strangerClient = _factory.CreateClient();
            var strangerToken = await strangerClient.RegisterAndGetTokenAsync("iris_wpupdatestranger");
            strangerClient.SetBearerToken(strangerToken);

            var updateResponse = await strangerClient.PutAsJsonAsync($"/api/workoutplans/{created!.Id}", PlanWithNestedTemplates(exercise.Id));

            Assert.Equal(HttpStatusCode.Unauthorized, updateResponse.StatusCode);
        }

        [Fact]
        public async Task UpdateWorkoutPlan_WhenPlanInUse_ReturnsConflict()
        {
            var exercise = await CreateExerciseAsAdminAsync("WP Cable Fly");
            var ownerToken = await _client.RegisterAndGetTokenAsync("iris_wpupdateinuse");
            _client.SetBearerToken(ownerToken);
            var createResponse = await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id, isPublic: true));
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();

            using var assigneeClient = _factory.CreateClient();
            var assigneeToken = await assigneeClient.RegisterAndGetTokenAsync("iris_wpupdateinuseassignee");
            assigneeClient.SetBearerToken(assigneeToken);
            await assigneeClient.PostAsync($"/api/workoutplans/{created!.Id}/assign", content: null);

            var updateResponse = await _client.PutAsJsonAsync($"/api/workoutplans/{created.Id}", PlanWithNestedTemplates(exercise.Id));

            Assert.Equal(HttpStatusCode.Conflict, updateResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteWorkoutPlan_WhenPlanInUse_ReturnsConflict()
        {
            // Regression test: DeleteWorkoutPlanCommandHandler used to delete unconditionally, and since
            // User.WorkoutPlanId -> WorkoutPlan is OnDelete(Restrict), deleting an in-use plan used to
            // throw a raw unhandled SqliteException instead of a clean 409.
            var exercise = await CreateExerciseAsAdminAsync("WP Leg Raise");
            var ownerToken = await _client.RegisterAndGetTokenAsync("iris_wpdeleteinuse");
            _client.SetBearerToken(ownerToken);
            var createResponse = await _client.PostAsJsonAsync("/api/workoutplans", PlanWithNestedTemplates(exercise.Id, isPublic: true));
            var created = await createResponse.Content.ReadFromJsonAsync<WorkoutPlanDto>();

            using var assigneeClient = _factory.CreateClient();
            var assigneeToken = await assigneeClient.RegisterAndGetTokenAsync("iris_wpdeleteinuseassignee");
            assigneeClient.SetBearerToken(assigneeToken);
            await assigneeClient.PostAsync($"/api/workoutplans/{created!.Id}/assign", content: null);

            var deleteResponse = await _client.DeleteAsync($"/api/workoutplans/{created.Id}");

            Assert.Equal(HttpStatusCode.Conflict, deleteResponse.StatusCode);
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
