using Application.Dto.Statistics;
using Application.Interfaces.Searchers;
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
        IUserSearcher userSearcher) : IRequestHandler<GetWorkoutStatisticsQuery, ErrorOr<StatisticsDto>>
    {
        private readonly IWorkoutSearcher _workoutSearcher = workoutSearcher;
        private readonly IExerciseSearcher _exerciseSearcher = exerciseSearcher;
        private readonly IUserSearcher _userSearcher = userSearcher;

        public async Task<ErrorOr<StatisticsDto>> Handle(GetWorkoutStatisticsQuery request, CancellationToken cancellationToken)
        {
            var workoutStartDate = await _userSearcher.GetWorkoutStartDateAsync(request.UserId, cancellationToken);
            if (workoutStartDate is null)
                return Errors.User.NotFound;

            var workouts = await _workoutSearcher.FindByUserIdAsync(request.UserId, cancellationToken);
            var exercises = await _exerciseSearcher.GetAllAsync(cancellationToken);

            var workoutExerciseEntries = workouts
                .SelectMany(w => w.WorkoutExercises.Select(we => new WorkoutExerciseEntry(we.Sets, we.ExerciseId)))
                .ToList();

            var exerciseEntries = exercises
                .Select(e => new ExerciseEntry(e.Id, e.Name))
                .ToList();

            return StatisticsCalculator.Calculate(
                workouts.Count,
                workoutExerciseEntries,
                workoutStartDate.Value,
                exerciseEntries).ToDto();
        }
    }
}
