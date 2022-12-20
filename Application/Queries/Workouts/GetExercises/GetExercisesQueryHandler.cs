using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Workouts.GetExercises
{
    public class GetExercisesQueryHandler : IRequestHandler<GetExercisesQuery, IEnumerable<ExerciseDto>>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IMapper _mapper;

        public GetExercisesQueryHandler(IWorkoutRepository workoutRepository, IMapper mapper)
        {
            _workoutRepository = workoutRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExerciseDto>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
        {
            var workouts = await _workoutRepository.GetExercisesAsync();

            return _mapper.Map<IEnumerable<ExerciseDto>>(workouts);
        }
    }
}