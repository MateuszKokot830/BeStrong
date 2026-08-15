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

            var result = await _sut.GetPagedAsync(new WorkoutPlanSearchCriteria(), owner.Id, null, CancellationToken.None);

            Assert.Equal(2, result.TotalItems);
            Assert.DoesNotContain(result, p => p.Name == "Others Private Plan");
        }

        [Fact]
        public async Task GetPagedAsync_WhenOnlyMyself_ExcludesOthersPublicPlans()
        {
            var owner = await CreateUserAsync("owner");
            var other = await CreateUserAsync("other");
            Context.WorkoutPlans.AddRange(
                new WorkoutPlan { CreatedById = other.Id, Name = "Public Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = true },
                new WorkoutPlan { CreatedById = owner.Id, Name = "My Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = false });
            await Context.SaveChangesAsync();

            var result = await _sut.GetPagedAsync(new WorkoutPlanSearchCriteria { CreatedBy = CreatedByFilter.OnlyMyself }, owner.Id, null, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("My Plan", result[0].Name);
        }

        [Fact]
        public async Task GetPagedAsync_WhenOnlyFollowers_ReturnsOnlyPublicPlansFromFollowedUsers()
        {
            var requester = await CreateUserAsync("requester");
            var followed = await CreateUserAsync("followed");
            var notFollowed = await CreateUserAsync("notfollowed");
            Context.WorkoutPlans.AddRange(
                new WorkoutPlan { CreatedById = followed.Id, Name = "Followed Public Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = true },
                new WorkoutPlan { CreatedById = followed.Id, Name = "Followed Private Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = false },
                new WorkoutPlan { CreatedById = notFollowed.Id, Name = "Not Followed Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = true },
                new WorkoutPlan { CreatedById = requester.Id, Name = "My Own Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = true });
            await Context.SaveChangesAsync();

            var result = await _sut.GetPagedAsync(
                new WorkoutPlanSearchCriteria { CreatedBy = CreatedByFilter.OnlyFollowers },
                requester.Id,
                [followed.Id],
                CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Followed Public Plan", result[0].Name);
        }

        [Fact]
        public async Task GetPagedAsync_FiltersByCategory()
        {
            var user = await CreateUserAsync();
            Context.WorkoutPlans.AddRange(
                new WorkoutPlan { CreatedById = user.Id, Name = "Full Body Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = true },
                new WorkoutPlan { CreatedById = user.Id, Name = "PPL Plan", Category = WorkoutPlanCategory.PushPullLegs, IsPublic = true });
            await Context.SaveChangesAsync();

            var result = await _sut.GetPagedAsync(new WorkoutPlanSearchCriteria { Category = WorkoutPlanCategory.PushPullLegs }, user.Id, null, CancellationToken.None);

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

            var result = await _sut.GetPagedAsync(new WorkoutPlanSearchCriteria { Name = "summer" }, user.Id, null, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Summer Shred", result[0].Name);
        }

        [Fact]
        public async Task GetPagedAsync_FiltersByOwnerNameSubstring_CaseInsensitive()
        {
            var jane = await CreateUserAsync("jane");
            jane.Name = "Jane";
            jane.Surname = "Doe";
            var john = await CreateUserAsync("john");
            john.Name = "John";
            john.Surname = "Smith";
            await Context.SaveChangesAsync();

            Context.WorkoutPlans.AddRange(
                new WorkoutPlan { CreatedById = jane.Id, Name = "Jane's Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = true },
                new WorkoutPlan { CreatedById = john.Id, Name = "John's Plan", Category = WorkoutPlanCategory.FullBody, IsPublic = true });
            await Context.SaveChangesAsync();

            var result = await _sut.GetPagedAsync(new WorkoutPlanSearchCriteria { OwnerName = "jane doe" }, jane.Id, null, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Jane's Plan", result[0].Name);
        }
    }
}
