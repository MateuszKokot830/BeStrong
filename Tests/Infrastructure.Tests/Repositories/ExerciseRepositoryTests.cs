using Domain.Common;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Tests.TestDoubles;

namespace Infrastructure.Tests.Repositories
{
    public class ExerciseRepositoryTests : SqliteInMemoryFixture
    {
        private readonly ExerciseRepository _sut;

        public ExerciseRepositoryTests()
        {
            _sut = new ExerciseRepository(Context);
        }

        [Fact]
        public async Task AddAsync_ThenSaveChanges_PersistsTheExercise()
        {
            var exercise = new Exercise { Name = "Squat", MuscleSubgroup = MuscleSubgroup.Quads };

            await _sut.AddAsync(exercise, CancellationToken.None);
            await Context.SaveChangesAsync();

            var loaded = await _sut.GetByIdAsync(exercise.Id, CancellationToken.None);
            Assert.NotNull(loaded);
            Assert.Equal("Squat", loaded!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WhenExerciseDoesNotExist_ReturnsNull()
        {
            var loaded = await _sut.GetByIdAsync(999, CancellationToken.None);

            Assert.Null(loaded);
        }

        [Fact]
        public async Task UpdateAsync_ThenSaveChanges_PersistsChanges()
        {
            var exercise = new Exercise { Name = "Old Name", MuscleSubgroup = MuscleSubgroup.Quads };
            await _sut.AddAsync(exercise, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var toUpdate = await _sut.GetByIdAsync(exercise.Id, CancellationToken.None);
            toUpdate!.Name = "New Name";
            await _sut.UpdateAsync(toUpdate, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var reloaded = await _sut.GetByIdAsync(exercise.Id, CancellationToken.None);
            Assert.Equal("New Name", reloaded!.Name);
        }

        [Fact]
        public async Task DeleteAsync_ThenSaveChanges_RemovesTheExercise()
        {
            var exercise = new Exercise { Name = "Squat", MuscleSubgroup = MuscleSubgroup.Quads };
            await _sut.AddAsync(exercise, CancellationToken.None);
            await Context.SaveChangesAsync();

            await _sut.DeleteAsync(exercise, CancellationToken.None);
            await Context.SaveChangesAsync();

            var loaded = await _sut.GetByIdAsync(exercise.Id, CancellationToken.None);
            Assert.Null(loaded);
        }
    }
}
