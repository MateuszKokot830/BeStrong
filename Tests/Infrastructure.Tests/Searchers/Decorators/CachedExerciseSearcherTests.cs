using Application.Dto.Exercise;
using Application.Interfaces.Searchers;
using Domain.Common;
using Infrastructure.Searchers.Decorators;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Infrastructure.Tests.Searchers.Decorators
{
    public class CachedExerciseSearcherTests
    {
        private readonly Mock<IExerciseSearcher> _inner = new();
        private readonly MemoryCache _cache = new(new MemoryCacheOptions());
        private CachedExerciseSearcher CreateSut() => new(_inner.Object, _cache);

        [Fact]
        public async Task GetAllAsync_OnCacheMiss_CallsInnerAndReturnsItsResult()
        {
            var exercises = new List<ExerciseDto> { new(1, "Bench Press", null, MuscleGroup.Chest, MuscleSubgroup.Chest, null) };
            _inner.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(exercises);

            var result = await CreateSut().GetAllAsync(CancellationToken.None);

            Assert.Same(exercises, result);
        }

        [Fact]
        public async Task GetAllAsync_OnCacheHit_DoesNotCallInnerAgain()
        {
            var exercises = new List<ExerciseDto> { new(1, "Bench Press", null, MuscleGroup.Chest, MuscleSubgroup.Chest, null) };
            _inner.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(exercises);
            var sut = CreateSut();

            await sut.GetAllAsync(CancellationToken.None);
            await sut.GetAllAsync(CancellationToken.None);

            _inner.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_CachesUnderTheDocumentedCacheKey()
        {
            var exercises = new List<ExerciseDto> { new(1, "Bench Press", null, MuscleGroup.Chest, MuscleSubgroup.Chest, null) };
            _inner.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(exercises);

            await CreateSut().GetAllAsync(CancellationToken.None);

            Assert.True(_cache.TryGetValue(CachedExerciseSearcher.CacheKey, out _));
        }
    }
}
