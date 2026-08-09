using Domain.Aggregates;
using Domain.Common;
using Infrastructure.Repositories;
using Infrastructure.Tests.TestDoubles;

namespace Infrastructure.Tests.Repositories
{
    public class WorkoutPlanRepositoryTests : SqliteInMemoryFixture
    {
        private readonly WorkoutPlanRepository _sut;

        public WorkoutPlanRepositoryTests()
        {
            _sut = new WorkoutPlanRepository(Context);
        }

        [Fact]
        public async Task AddAsync_ThenSaveChanges_PersistsThePlan()
        {
            var user = await CreateUserAsync();
            var plan = new WorkoutPlan { CreatedById = user.Id, Name = "PPL", Category = WorkoutPlanCategory.PushPullLegs };

            await _sut.AddAsync(plan, CancellationToken.None);
            await Context.SaveChangesAsync();

            var loaded = await _sut.GetByIdAsync(plan.Id, CancellationToken.None);
            Assert.NotNull(loaded);
            Assert.Equal("PPL", loaded!.Name);
        }

        [Fact]
        public async Task GetUserCurrentWorkoutPlanAsync_IncludesTemplatesAndExercises()
        {
            var user = await CreateUserAsync();
            var exercise = await CreateExerciseAsync();
            var plan = new WorkoutPlan
            {
                CreatedById = user.Id,
                Name = "PPL",
                Category = WorkoutPlanCategory.PushPullLegs,
                WorkoutTemplates =
                [
                    new Domain.Entities.WorkoutTemplate
                    {
                        Name = "Day A",
                        Exercises = [new Domain.Entities.WorkoutTemplateExercise { ExerciseId = exercise.Id }]
                    }
                ]
            };
            await _sut.AddAsync(plan, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var loaded = await _sut.GetUserCurrentWorkoutPlanAsync(plan.Id, CancellationToken.None);

            Assert.Single(loaded!.WorkoutTemplates);
            Assert.Single(loaded.WorkoutTemplates.First().Exercises);
        }

        [Fact]
        public async Task GetUserCurrentWorkoutPlanAsync_WhenPlanDoesNotExist_ReturnsNull()
        {
            var loaded = await _sut.GetUserCurrentWorkoutPlanAsync(999, CancellationToken.None);

            Assert.Null(loaded);
        }

        [Fact]
        public async Task DeleteAsync_ThenSaveChanges_RemovesThePlan()
        {
            var user = await CreateUserAsync();
            var plan = new WorkoutPlan { CreatedById = user.Id, Name = "PPL", Category = WorkoutPlanCategory.PushPullLegs };
            await _sut.AddAsync(plan, CancellationToken.None);
            await Context.SaveChangesAsync();

            await _sut.DeleteAsync(plan, CancellationToken.None);
            await Context.SaveChangesAsync();

            var loaded = await _sut.GetByIdAsync(plan.Id, CancellationToken.None);
            Assert.Null(loaded);
        }
    }
}
