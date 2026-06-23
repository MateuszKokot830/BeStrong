using Application.Dto.Exercise;
using Application.Interfaces.Searchers;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Searchers.Decorators
{
    public sealed class CachedExerciseSearcher(IExerciseSearcher inner, IMemoryCache cache) : IExerciseSearcher
    {
        public const string CacheKey = "exercises:all";

        public async Task<IReadOnlyList<ExerciseDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await cache.GetOrCreateAsync(CacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return inner.GetAllAsync(cancellationToken);
            }) ?? [];
    }
}
