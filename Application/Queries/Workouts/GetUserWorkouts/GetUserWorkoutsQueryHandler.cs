using Application.Dto.Workout;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Workouts.GetUserWorkouts
{
    public class GetUserWorkoutsQueryHandler(IWorkoutRepository workoutRepository, IMapper mapper) : IRequestHandler<GetUserWorkoutsQuery, IEnumerable<WorkoutDto>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<WorkoutDto>> Handle(GetUserWorkoutsQuery request, CancellationToken cancellationToken)
        {
            var workouts = await _workoutRepository.GetUserWorkoutsAsync(request.UserId);

            return _mapper.Map<IEnumerable<WorkoutDto>>(workouts);
        }
    }
}