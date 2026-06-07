using Application.Dto.Workout;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetUserWorkouts
{
    public class GetUserWorkoutsQueryHandler(IWorkoutRepository workoutRepository, IMapper mapper) : IRequestHandler<GetUserWorkoutsQuery, ErrorOr<IEnumerable<WorkoutDto>>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<IEnumerable<WorkoutDto>>> Handle(GetUserWorkoutsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var workouts = await _workoutRepository.GetUserWorkoutsAsync(request.UserId, cancellationToken);
                return _mapper.Map<IEnumerable<WorkoutDto>>(workouts).ToList();
            }
            catch (Exception)
            {
                return Errors.Workout.NotFound;
            }
        }
    }
}
