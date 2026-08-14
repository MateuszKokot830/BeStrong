using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Domain.Common;
using Domain.Factories;
using MediatR;

namespace Application.Notifications
{
    public sealed class WorkoutSavedNotificationHandler(
        IPostRepository postRepository,
        IUnitOfWork unitOfWork) : INotificationHandler<WorkoutSavedNotification>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(WorkoutSavedNotification notification, CancellationToken cancellationToken)
        {
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
