using Application.Dto.Exercise;
using Application.Interfaces.Repositories;
using Application.Mappings;
using Application.Notifications;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.UpdateExercise
{
    public class UpdateExerciseCommandHandler(IExerciseRepository exerciseRepository, IPublisher publisher)
        : IRequestHandler<UpdateExerciseCommand, ErrorOr<ExerciseDto>>
    {
        private readonly IExerciseRepository _exerciseRepository = exerciseRepository;
        private readonly IPublisher _publisher = publisher;

        public async Task<ErrorOr<ExerciseDto>> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(request.ExerciseId, cancellationToken);
            if (exercise is null)
                return Errors.Exercise.NotFound;

            exercise.Name = request.ExerciseDto.Name;
            exercise.Description = request.ExerciseDto.Description;
            exercise.MuscleSubgroup = request.ExerciseDto.MuscleSubgroup;
            exercise.ImageUrl = request.ExerciseDto.ImageUrl;

            await _exerciseRepository.UpdateAsync(exercise, cancellationToken);
            await _publisher.Publish(new ExerciseChangedNotification(), cancellationToken);
            return exercise.ToDto();
        }
    }
}
