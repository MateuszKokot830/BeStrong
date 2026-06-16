using Application.Dto.Exercise;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Commands.Workouts.CreateExercise
{
    public class CreateExerciseCommandHandler(IWorkoutRepository workoutRepository)
        : IRequestHandler<CreateExerciseCommand, ErrorOr<ExerciseDto>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;

        public async Task<ErrorOr<ExerciseDto>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            var exercise = request.ExerciseDto.ToEntity();
            await _workoutRepository.CreateExerciseAsync(exercise, cancellationToken);
            return exercise.ToDto();
        }
    }
}
