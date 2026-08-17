using Application.Helpers.Criteria;
using Domain.Aggregates;
using Domain.Entities;
using Infrastructure.Searchers;
using Infrastructure.Tests.TestDoubles;

namespace Infrastructure.Tests.Searchers
{
    public class WorkoutSearcherTests : SqliteInMemoryFixture
    {
        private readonly WorkoutSearcher _sut;

        public WorkoutSearcherTests()
        {
            _sut = new WorkoutSearcher(Context);
        }

        [Fact]
        public async Task FindByUserIdAsync_OnlyReturnsThatUsersWorkouts_OrderedByDateDescending()
        {
            var user = await CreateUserAsync();
            var otherUser = await CreateUserAsync("bob");
            var older = new Workout { UserId = user.Id, Name = "Older", Date = DateTime.UtcNow.AddDays(-1) };
            var newer = new Workout { UserId = user.Id, Name = "Newer", Date = DateTime.UtcNow };
            Context.Workouts.AddRange(older, newer, new Workout { UserId = otherUser.Id, Name = "Not mine", Date = DateTime.UtcNow });
            await Context.SaveChangesAsync();

            var result = await _sut.FindByUserIdAsync(user.Id, CancellationToken.None);

            Assert.Equal(["Newer", "Older"], result.Select(w => w.Name));
        }

        [Fact]
        public async Task FindByUserIdAsync_IncludesWorkoutExercisesAndSets()
        {
            var user = await CreateUserAsync();
            var exercise = await CreateExerciseAsync();
            var workout = new Workout
            {
                UserId = user.Id,
                Name = "Push Day",
                Date = DateTime.UtcNow,
                WorkoutExercises = [new WorkoutExercise { ExerciseId = exercise.Id, Sets = [new WorkoutSet { SetNumber = 1, Reps = 10, Weight = 50 }] }]
            };
            Context.Workouts.Add(workout);
            await Context.SaveChangesAsync();

            var result = await _sut.FindByUserIdAsync(user.Id, CancellationToken.None);

            Assert.Single(result[0].WorkoutExercises);
            Assert.Single(result[0].WorkoutExercises.First().Sets);
        }

        [Fact]
        public async Task FindByUserIdAsync_WhenUserHasNoWorkouts_ReturnsEmptyList()
        {
            var user = await CreateUserAsync();

            var result = await _sut.FindByUserIdAsync(user.Id, CancellationToken.None);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPagedAsync_OnlyReturnsThatUsersWorkouts_OrderedByDateDescending()
        {
            var user = await CreateUserAsync();
            var otherUser = await CreateUserAsync("bob");
            var older = new Workout { UserId = user.Id, Name = "Older", Date = DateTime.UtcNow.AddDays(-1) };
            var newer = new Workout { UserId = user.Id, Name = "Newer", Date = DateTime.UtcNow };
            Context.Workouts.AddRange(older, newer, new Workout { UserId = otherUser.Id, Name = "Not mine", Date = DateTime.UtcNow });
            await Context.SaveChangesAsync();

            var result = await _sut.GetPagedAsync(new WorkoutSearchCriteria(), user.Id, CancellationToken.None);

            Assert.Equal(["Newer", "Older"], result.Select(w => w.Name));
        }

        [Fact]
        public async Task GetPagedAsync_FiltersByDateRange()
        {
            var user = await CreateUserAsync();
            var inRange = new Workout { UserId = user.Id, Name = "In range", Date = new DateTime(2026, 6, 15) };
            var beforeRange = new Workout { UserId = user.Id, Name = "Before range", Date = new DateTime(2026, 5, 1) };
            var afterRange = new Workout { UserId = user.Id, Name = "After range", Date = new DateTime(2026, 7, 1) };
            Context.Workouts.AddRange(inRange, beforeRange, afterRange);
            await Context.SaveChangesAsync();

            var result = await _sut.GetPagedAsync(
                new WorkoutSearchCriteria { DateFrom = new DateTime(2026, 6, 1), DateTo = new DateTime(2026, 6, 30) },
                user.Id, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("In range", result[0].Name);
        }

        [Fact]
        public async Task GetPagedAsync_FiltersByNameSubstring_CaseInsensitive()
        {
            var user = await CreateUserAsync();
            Context.Workouts.AddRange(
                new Workout { UserId = user.Id, Name = "Push Day", Date = DateTime.UtcNow },
                new Workout { UserId = user.Id, Name = "Pull Day", Date = DateTime.UtcNow });
            await Context.SaveChangesAsync();

            var result = await _sut.GetPagedAsync(new WorkoutSearchCriteria { Name = "push" }, user.Id, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Push Day", result[0].Name);
        }

        [Fact]
        public async Task GetPagedAsync_FiltersByExerciseId()
        {
            var user = await CreateUserAsync();
            var benchPress = await CreateExerciseAsync("Bench Press");
            var squat = await CreateExerciseAsync("Squat");
            Context.Workouts.AddRange(
                new Workout
                {
                    UserId = user.Id,
                    Name = "Chest Day",
                    Date = DateTime.UtcNow,
                    WorkoutExercises = [new WorkoutExercise { ExerciseId = benchPress.Id }]
                },
                new Workout
                {
                    UserId = user.Id,
                    Name = "Leg Day",
                    Date = DateTime.UtcNow,
                    WorkoutExercises = [new WorkoutExercise { ExerciseId = squat.Id }]
                });
            await Context.SaveChangesAsync();

            var result = await _sut.GetPagedAsync(new WorkoutSearchCriteria { ExerciseId = benchPress.Id }, user.Id, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Chest Day", result[0].Name);
        }
    }
}
