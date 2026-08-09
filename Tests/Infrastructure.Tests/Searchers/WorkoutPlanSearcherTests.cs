using Domain.Aggregates;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Searchers;
using Infrastructure.Tests.TestDoubles;

namespace Infrastructure.Tests.Searchers
{
    public class WorkoutPlanSearcherTests : SqliteInMemoryFixture
    {
        private readonly WorkoutPlanSearcher _sut;

        public WorkoutPlanSearcherTests()
        {
            _sut = new WorkoutPlanSearcher(Context);
        }

        [Fact]
        public async Task FindByIdAsync_WhenPlanDoesNotExist_ReturnsNull()
        {
            var result = await _sut.FindByIdAsync(999, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task FindByIdAsync_IncludesTemplatesAndExercises()
        {
            var user = await CreateUserAsync();
            var exercise = await CreateExerciseAsync();
            var plan = new WorkoutPlan
            {
                CreatedById = user.Id,
                Name = "PPL",
                Category = WorkoutPlanCategory.PushPullLegs,
                WorkoutTemplates = [new WorkoutTemplate { Name = "Day A", Exercises = [new WorkoutTemplateExercise { ExerciseId = exercise.Id }] }]
            };
            Context.WorkoutPlans.Add(plan);
            await Context.SaveChangesAsync();

            var result = await _sut.FindByIdAsync(plan.Id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result!.WorkoutTemplates);
            Assert.Single(result.WorkoutTemplates.First().Exercises);
        }

        [Fact]
        public async Task GetPublicAsync_OnlyReturnsPublicPlans()
        {
            var user = await CreateUserAsync();
            Context.WorkoutPlans.AddRange(
                new WorkoutPlan { CreatedById = user.Id, Name = "Public Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = true },
                new WorkoutPlan { CreatedById = user.Id, Name = "Private Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = false });
            await Context.SaveChangesAsync();

            var result = await _sut.GetPublicAsync(CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Public Plan", result[0].Name);
        }
    }
}
