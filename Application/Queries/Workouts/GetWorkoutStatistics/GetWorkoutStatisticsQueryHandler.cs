using Application.Dto.Statistics;
using Application.Interfaces.Repositories;
using Application.Mappings;
using Domain.Errors;
using Domain.Services;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetWorkoutStatistics
{
    public class GetWorkoutStatisticsQueryHandler(
        IWorkoutRepository workoutRepository,
        IUserRepository userRepository) : IRequestHandler<GetWorkoutStatisticsQuery, ErrorOr<StatisticsDto>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ErrorOr<StatisticsDto>> Handle(GetWorkoutStatisticsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            var workouts = await _workoutRepository.GetUserWorkoutsAsync(request.UserId, cancellationToken);
            var exercises = await _workoutRepository.GetExercisesAsync(cancellationToken);

            return StatisticsCalculator.Calculate(workouts, user.DateOfWorkoutStart, exercises).ToDto();
        }
    }
}
