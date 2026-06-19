using Application.Dto.Exercise;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.CreateExercise
{
    public class CreateExerciseCommandHandler(IExerciseRepository exerciseRepository)
        : IRequestHandler<CreateExerciseCommand, ErrorOr<ExerciseDto>>
    {
        private readonly IExerciseRepository _exerciseRepository = exerciseRepository;

        public async Task<ErrorOr<ExerciseDto>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            var exercise = request.ExerciseDto.ToEntity();
            await _exerciseRepository.AddAsync(exercise, cancellationToken);
            return exercise.ToDto();
        }
    }
}
