using Application.Dto.Exercise;
using Application.Interfaces.Repositories;
using AutoMapper;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetExercises
{
    public class GetExercisesQueryHandler(IWorkoutRepository workoutRepository, IMapper mapper)
        : IRequestHandler<GetExercisesQuery, ErrorOr<IEnumerable<ExerciseDto>>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<IEnumerable<ExerciseDto>>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
        {
            var exercises = await _workoutRepository.GetExercisesAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ExerciseDto>>(exercises).ToList();
        }
    }
}
