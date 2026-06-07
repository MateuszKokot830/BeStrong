using Application.Dto.Statistics;
using Application.Dto.Workout;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Workouts.GetWorkoutStatistics
{
    public class GetWorkoutStatisticsQueryHandler(IWorkoutRepository workoutRepository,
                                            IUserRepository userRepository,
                                            IMapper mapper) : IRequestHandler<GetWorkoutStatisticsQuery, ErrorOr<StatisticsDto>>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<StatisticsDto>> Handle(GetWorkoutStatisticsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            var workouts = await _workoutRepository.GetUserWorkoutsAsync(request.UserId, cancellationToken);
            if (workouts is null)
                return Errors.Workout.NotFound;

            var workoutsDto = _mapper.Map<IEnumerable<WorkoutDto>>(workouts).ToList();
            var totalWorkouts = workoutsDto.Count();
            var exercises = workoutsDto.SelectMany(x => x.WorkoutExercises).ToList();
            var totalExercises = exercises.Count();
            var totalSets = workoutsDto.SelectMany(x => x.WorkoutExercises).Sum(y => y.Sets);
            var avgWorkoutsPerWeek = (decimal)(totalWorkouts / ((DateTime.Now.Day - user.DateOfWorkoutStart.Day) / 7));
            var avgExercisesPerWorkout = (decimal)(totalExercises / totalWorkouts);
            var avgSetsPerWorkout = (decimal)(totalSets / totalWorkouts);

            var groupedExercises = exercises.GroupBy(x => x.ExerciseId)
                .Select(y => new { Id = y.Key, Count = y.Count() })
                .ToList();

            var favouriteExerciseId = groupedExercises.OrderByDescending(x => x.Count).FirstOrDefault();
            var exercisesList = await _workoutRepository.GetExercisesAsync(cancellationToken);
            var favouriteExercise = exercisesList
                .Where(x => x.Id == favouriteExerciseId?.Id)
                .Select(x => x.Name)
                .FirstOrDefault();

            return new StatisticsDto(
                totalWorkouts,
                totalExercises,
                totalSets,
                avgWorkoutsPerWeek,
                avgExercisesPerWorkout,
                avgSetsPerWorkout,
                favouriteExercise);
        }
    }
}
