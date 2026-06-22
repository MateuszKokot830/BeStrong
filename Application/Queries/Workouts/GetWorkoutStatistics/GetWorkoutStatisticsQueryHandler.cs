using Application.Dto.Statistics;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Errors;
using Domain.Services;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetWorkoutStatistics
{
    public class GetWorkoutStatisticsQueryHandler(
        IWorkoutSearcher workoutSearcher,
        IExerciseSearcher exerciseSearcher,
        IUserSearcher userSearcher,
        ICurrentUserService currentUserService) : IRequestHandler<GetWorkoutStatisticsQuery, ErrorOr<StatisticsDto>>
    {
        private readonly IWorkoutSearcher _workoutSearcher = workoutSearcher;
        private readonly IExerciseSearcher _exerciseSearcher = exerciseSearcher;
        private readonly IUserSearcher _userSearcher = userSearcher;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<StatisticsDto>> Handle(GetWorkoutStatisticsQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsOwnerOrAdmin(request.UserId))
                return Errors.User.Unauthorized;

            if (!await _userSearcher.ExistsAsync(request.UserId, cancellationToken))
                return Errors.User.NotFound;

            var workoutStartDate = await _userSearcher.GetWorkoutStartDateAsync(request.UserId, cancellationToken);
            var workouts = await _workoutSearcher.FindByUserIdAsync(request.UserId, cancellationToken);
            var exercises = await _exerciseSearcher.GetAllAsync(cancellationToken);

            var workoutExerciseEntries = workouts
                .SelectMany(w => w.WorkoutExercises.Select(we => new WorkoutExerciseEntry(we.Sets.Count, we.ExerciseId)))
                .ToList();

            var exerciseEntries = exercises
                .Select(e => new ExerciseEntry(e.Id, e.Name))
                .ToList();

            return StatisticsCalculator.Calculate(
                workouts.Count,
                workoutExerciseEntries,
                workoutStartDate,
                exerciseEntries).ToDto();
        }
    }
}
