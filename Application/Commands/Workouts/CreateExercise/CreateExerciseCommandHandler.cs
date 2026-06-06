using AutoMapper;
using MediatR;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Commands.Workouts.CreateExercise
{
    public class CreateExerciseCommandHandler(IWorkoutRepository workoutRepository, IMapper mapper) : IRequestHandler<CreateExerciseCommand>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            var exercise = _mapper.Map<Exercise>(request.ExerciseDto);
            await _workoutRepository.CreateExerciseAsync(exercise);

            return Unit.Value;
        }
    }
}