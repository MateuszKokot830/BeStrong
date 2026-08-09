using Domain.Common;
using Domain.Entities;
using Infrastructure.Searchers;
using Infrastructure.Tests.TestDoubles;

namespace Infrastructure.Tests.Searchers
{
    public class ExerciseSearcherTests : SqliteInMemoryFixture
    {
        private readonly ExerciseSearcher _sut;

        public ExerciseSearcherTests()
        {
            _sut = new ExerciseSearcher(Context);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoExercisesExist_ReturnsEmptyList()
        {
            var result = await _sut.GetAllAsync(CancellationToken.None);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllExercisesMappedToDto()
        {
            Context.Excercises.AddRange(
                new Exercise { Name = "Bench Press", MuscleSubgroup = MuscleSubgroup.Chest },
                new Exercise { Name = "Squat", MuscleSubgroup = MuscleSubgroup.Quads });
            await Context.SaveChangesAsync();

            var result = await _sut.GetAllAsync(CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.Name == "Bench Press" && e.MuscleGroup == MuscleGroup.Chest);
            Assert.Contains(result, e => e.Name == "Squat" && e.MuscleGroup == MuscleGroup.Legs);
        }
    }
}
