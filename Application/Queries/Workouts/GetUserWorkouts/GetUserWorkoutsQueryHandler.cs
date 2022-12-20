using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Workouts.GetUserWorkouts
{
    public class GetUserWorkoutsQueryHandler : IRequestHandler<GetUserWorkoutsQuery, IEnumerable<WorkoutDto>>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IMapper _mapper;

        public GetUserWorkoutsQueryHandler(IWorkoutRepository workoutRepository, IMapper mapper)
        {
            _workoutRepository = workoutRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkoutDto>> Handle(GetUserWorkoutsQuery request, CancellationToken cancellationToken)
        {
            var workouts = await _workoutRepository.GetUserWorkoutsAsync(request.UserId);

            return _mapper.Map<IEnumerable<WorkoutDto>>(workouts);
        }
    }
}