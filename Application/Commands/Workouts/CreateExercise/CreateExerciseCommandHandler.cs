using Application.Dto.Exercise;
using Application.Interfaces.Repositories;
using Application.Mappings;
using Application.Notifications;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.CreateExercise
{
    public class CreateExerciseCommandHandler(IExerciseRepository exerciseRepository, IPublisher publisher)
        : IRequestHandler<CreateExerciseCommand, ErrorOr<ExerciseDto>>
    {
        private readonly IExerciseRepository _exerciseRepository = exerciseRepository;
        private readonly IPublisher _publisher = publisher;

        public async Task<ErrorOr<ExerciseDto>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            var exercise = request.ExerciseDto.ToEntity();
            await _exerciseRepository.AddAsync(exercise, cancellationToken);
            await _publisher.Publish(new ExerciseChangedNotification(), cancellationToken);
            return exercise.ToDto();
        }
    }
}
