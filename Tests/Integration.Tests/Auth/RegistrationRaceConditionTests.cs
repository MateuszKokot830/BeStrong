using System.Net;
using System.Net.Http.Json;
using Application.Dto.Auth;

namespace Integration.Tests.Auth
{
    public class RegistrationRaceConditionTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;

        public RegistrationRaceConditionTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ConcurrentRegistrations_WithTheSameUsername_OnlyOneSucceeds()
        {
            // RegisterCommandHandler's duplicate check (GetByUsernameAsync then CreateAsync) is a
            // classic check-then-act race: two requests can both pass the initial check before either
            // has committed. This doesn't assert *which* one wins — only that the outcome is exactly
            // one success and the loser fails gracefully (never an unhandled 500), since either
            // Identity's own uniqueness validation or a DB-level unique constraint should still be the
            // real backstop even if the handler's own check isn't atomic.
            using var clientA = _factory.CreateClient();
            using var clientB = _factory.CreateClient();
            var dto = new UserRegisterRequestDto("race_condition_user", "Password1!");

            var taskA = clientA.PostAsJsonAsync("/api/auth/register", dto);
            var taskB = clientB.PostAsJsonAsync("/api/auth/register", dto);
            var responses = await Task.WhenAll(taskA, taskB);

            var successCount = responses.Count(r => r.StatusCode == HttpStatusCode.OK);
            Assert.Equal(1, successCount);

            foreach (var response in responses)
            {
                Assert.NotEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            }
        }

        [Fact]
        public async Task ConcurrentRegistrations_WithTheSameUsername_LoserGetsAGracefulConflictOrValidationError()
        {
            using var clientA = _factory.CreateClient();
            using var clientB = _factory.CreateClient();
            var dto = new UserRegisterRequestDto("race_condition_loser_check", "Password1!");

            var responses = await Task.WhenAll(
                clientA.PostAsJsonAsync("/api/auth/register", dto),
                clientB.PostAsJsonAsync("/api/auth/register", dto));

            var loser = responses.Single(r => r.StatusCode != HttpStatusCode.OK);
            Assert.True(
                loser.StatusCode is HttpStatusCode.Conflict or HttpStatusCode.UnprocessableEntity,
                $"Expected the losing request to fail gracefully with 409 or 422, but got {loser.StatusCode}: {await loser.Content.ReadAsStringAsync()}");
        }
    }
}
