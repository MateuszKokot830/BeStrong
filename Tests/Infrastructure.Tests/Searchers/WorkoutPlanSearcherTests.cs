using Application.Helpers.Criteria;
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
        public async Task GetPagedAsync_ReturnsPublicAndOwnPlans_ExcludesOthersPrivatePlans()
        {
            var owner = await CreateUserAsync("owner");
            var other = await CreateUserAsync("other");
            Context.WorkoutPlans.AddRange(
                new WorkoutPlan { CreatedById = other.Id, Name = "Public Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = true },
                new WorkoutPlan { CreatedById = owner.Id, Name = "My Private Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = false },
                new WorkoutPlan { CreatedById = other.Id, Name = "Others Private Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = false });
            await Context.SaveChangesAsync();

            var result = await _sut.GetPagedAsync(new WorkoutPlanSearchCriteria(), owner.Id, CancellationToken.None);

            Assert.Equal(2, result.TotalItems);
            Assert.DoesNotContain(result, p => p.Name == "Others Private Plan");
        }

        [Fact]
        public async Task GetPagedAsync_WhenOnlyOwn_ExcludesOthersPublicPlans()
        {
            var owner = await CreateUserAsync("owner");
            var other = await CreateUserAsync("other");
            Context.WorkoutPlans.AddRange(
                new WorkoutPlan { CreatedById = other.Id, Name = "Public Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = true },
                new WorkoutPlan { CreatedById = owner.Id, Name = "My Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = false });
            await Context.SaveChangesAsync();

            var result = await _sut.GetPagedAsync(new WorkoutPlanSearchCriteria { OnlyOwn = true }, owner.Id, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("My Plan", result[0].Name);
        }

        [Fact]
        public async Task GetPagedAsync_FiltersByCategory()
        {
            var user = await CreateUserAsync();
            Context.WorkoutPlans.AddRange(
                new WorkoutPlan { CreatedById = user.Id, Name = "Full Body Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = true },
                new WorkoutPlan { CreatedById = user.Id, Name = "PPL Plan", Category = WorkoutPlanCategory.PushPullLegs, IsPublic = true });
            await Context.SaveChangesAsync();

            var result = await _sut.GetPagedAsync(new WorkoutPlanSearchCriteria { Category = WorkoutPlanCategory.PushPullLegs }, user.Id, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("PPL Plan", result[0].Name);
        }

        [Fact]
        public async Task GetPagedAsync_FiltersByNameSubstring_CaseInsensitive()
        {
            var user = await CreateUserAsync();
            Context.WorkoutPlans.AddRange(
                new WorkoutPlan { CreatedById = user.Id, Name = "Summer Shred", Category = WorkoutPlanCategory.FullBody, IsPublic = true },
                new WorkoutPlan { CreatedById = user.Id, Name = "Winter Bulk", Category = WorkoutPlanCategory.FullBody, IsPublic = true });
            await Context.SaveChangesAsync();

            var result = await _sut.GetPagedAsync(new WorkoutPlanSearchCriteria { Name = "summer" }, user.Id, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Summer Shred", result[0].Name);
        }
    }
}
