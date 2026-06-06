using Application.Dto.Exercise;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Workouts.GetExercises
{
    public class GetExercisesQueryHandler(IWorkoutRepository workoutRepository, IMapper mapper) : IRequestHandler<GetExercisesQuery, IEnumerable<ExerciseDto>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ExerciseDto>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
        {
            var workouts = await _workoutRepository.GetExercisesAsync();

            return _mapper.Map<IEnumerable<ExerciseDto>>(workouts);
        }
    }
}