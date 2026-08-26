using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Searchers;
using Domain.Common;
using Domain.Factories;
using MediatR;

namespace Application.Notifications
{
    public sealed class WorkoutSavedNotificationHandler(
        IPostRepository postRepository,
        IUserSearcher userSearcher,
        IUnitOfWork unitOfWork) : INotificationHandler<WorkoutSavedNotification>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IUserSearcher _userSearcher = userSearcher;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(WorkoutSavedNotification notification, CancellationToken cancellationToken)
        {
            var settings = await _userSearcher.GetSettingsAsync(notification.UserId, cancellationToken);
            if (!settings.AutoPublishWorkouts)
                return;

            var post = PostFactory.Create(
                notification.UserId,
                PostType.WorkoutPublication,
                notification.Description,
                notification.WorkoutId,
                workoutPlanId: null);

            await _postRepository.AddAsync(post, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
