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
    }
}
