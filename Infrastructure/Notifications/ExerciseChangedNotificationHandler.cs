using Application.Notifications;
using Infrastructure.Searchers.Decorators;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Notifications
{
    public sealed class ExerciseChangedNotificationHandler(IMemoryCache cache) : INotificationHandler<ExerciseChangedNotification>
    {
        public Task Handle(ExerciseChangedNotification notification, CancellationToken cancellationToken)
        {
            cache.Remove(CachedExerciseSearcher.CacheKey);
            return Task.CompletedTask;
        }
    }
}
