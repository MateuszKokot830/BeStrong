using AutoMapper;
using MediatR;
using Domain.Entities;
using Domain.Errors;
using ErrorOr;
using Application.Dto.Exercise;
using Application.Interfaces.Repositories;

namespace Application.Commands.Workouts.CreateExercise
{
    public class CreateExerciseCommandHandler(IWorkoutRepository workoutRepository, IMapper mapper) : IRequestHandler<CreateExerciseCommand, ErrorOr<ExerciseDto>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<ExerciseDto>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exercise = _mapper.Map<Exercise>(request.ExerciseDto);
                await _workoutRepository.CreateExerciseAsync(exercise, cancellationToken);
                return _mapper.Map<ExerciseDto>(exercise);
            }
            catch (Exception)
            {
                return Errors.Exercise.CreationFailed;
            }
        }
    }
}
