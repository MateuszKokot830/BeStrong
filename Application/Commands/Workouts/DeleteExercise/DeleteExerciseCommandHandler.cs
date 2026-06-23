using Application.Interfaces.Repositories;
using Application.Notifications;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.DeleteExercise
{
    public class DeleteExerciseCommandHandler(IExerciseRepository exerciseRepository, IPublisher publisher)
        : IRequestHandler<DeleteExerciseCommand, ErrorOr<Unit>>
    {
        private readonly IExerciseRepository _exerciseRepository = exerciseRepository;
        private readonly IPublisher _publisher = publisher;

        public async Task<ErrorOr<Unit>> Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(request.ExerciseId, cancellationToken);
            if (exercise is null)
                return Errors.Exercise.NotFound;

            await _exerciseRepository.DeleteAsync(exercise, cancellationToken);
            await _publisher.Publish(new ExerciseChangedNotification(), cancellationToken);
            return Unit.Value;
        }
    }
}
