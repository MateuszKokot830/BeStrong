using Domain.Aggregates;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Tests.TestDoubles;

namespace Infrastructure.Tests.Repositories
{
    public class WorkoutRepositoryTests : SqliteInMemoryFixture
    {
        private readonly WorkoutRepository _sut;

        public WorkoutRepositoryTests()
        {
            _sut = new WorkoutRepository(Context);
        }

        [Fact]
        public async Task AddAsync_ThenSaveChanges_PersistsTheWorkout()
        {
            var user = await CreateUserAsync();
            var workout = new Workout { UserId = user.Id, Name = "Push Day", Date = DateTime.UtcNow };

            await _sut.AddAsync(workout, CancellationToken.None);
            await Context.SaveChangesAsync();

            var loaded = await _sut.GetByIdAsync(workout.Id, CancellationToken.None);
            Assert.NotNull(loaded);
            Assert.Equal("Push Day", loaded!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WhenWorkoutDoesNotExist_ReturnsNull()
        {
            var loaded = await _sut.GetByIdAsync(999, CancellationToken.None);

            Assert.Null(loaded);
        }

        [Fact]
        public async Task GetByIdAsync_IncludesWorkoutExercisesAndSets()
        {
            var user = await CreateUserAsync();
            var exercise = await CreateExerciseAsync();
            var workout = new Workout
            {
                UserId = user.Id,
                Name = "Push Day",
                Date = DateTime.UtcNow,
                WorkoutExercises =
                [
                    new WorkoutExercise
                    {
                        ExerciseId = exercise.Id,
                        Sets = [new WorkoutSet { SetNumber = 1, Reps = 10, Weight = 50 }]
                    }
                ]
            };
            await _sut.AddAsync(workout, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var loaded = await _sut.GetByIdAsync(workout.Id, CancellationToken.None);

            Assert.Single(loaded!.WorkoutExercises);
            Assert.Single(loaded.WorkoutExercises.First().Sets);
        }

        [Fact]
        public async Task UpdateAsync_ThenSaveChanges_PersistsChanges()
        {
            var user = await CreateUserAsync();
            var workout = new Workout { UserId = user.Id, Name = "Old Name", Date = DateTime.UtcNow };
            await _sut.AddAsync(workout, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var toUpdate = await _sut.GetByIdAsync(workout.Id, CancellationToken.None);
            toUpdate!.Name = "New Name";
            await _sut.UpdateAsync(toUpdate, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var reloaded = await _sut.GetByIdAsync(workout.Id, CancellationToken.None);
            Assert.Equal("New Name", reloaded!.Name);
        }

        [Fact]
        public async Task DeleteAsync_ThenSaveChanges_RemovesTheWorkout()
        {
            var user = await CreateUserAsync();
            var workout = new Workout { UserId = user.Id, Name = "Push Day", Date = DateTime.UtcNow };
            await _sut.AddAsync(workout, CancellationToken.None);
            await Context.SaveChangesAsync();

            await _sut.DeleteAsync(workout, CancellationToken.None);
            await Context.SaveChangesAsync();

            var loaded = await _sut.GetByIdAsync(workout.Id, CancellationToken.None);
            Assert.Null(loaded);
        }
    }
}
