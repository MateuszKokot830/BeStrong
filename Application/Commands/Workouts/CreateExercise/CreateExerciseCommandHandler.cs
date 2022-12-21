using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Domain.Entities;

namespace Application.Commands.Workouts.CreateExercise
{
    public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IMapper _mapper;

        public CreateExerciseCommandHandler(IWorkoutRepository workoutRepository, IMapper mapper)
        {
            _workoutRepository = workoutRepository;
            _mapper = mapper;
        }

         public async Task<Unit> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {   
            var exercise = _mapper.Map<Exercise>(request.ExerciseDto);
            await _workoutRepository.CreateExerciseAsync(exercise);

            return Unit.Value;
        }
    }
}