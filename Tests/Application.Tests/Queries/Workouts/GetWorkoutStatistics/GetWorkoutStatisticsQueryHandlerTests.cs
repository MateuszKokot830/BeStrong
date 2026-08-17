using Application.Dto.Exercise;
using Application.Dto.Workout;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Application.Queries.Workouts.GetWorkoutStatistics;
using Domain.Common;
using Domain.Errors;
using Moq;

namespace Application.Tests.Queries.Workouts.GetWorkoutStatistics
{
    public class GetWorkoutStatisticsQueryHandlerTests
    {
        private readonly Mock<IWorkoutSearcher> _workoutSearcher = new();
        private readonly Mock<IExerciseSearcher> _exerciseSearcher = new();
        private readonly Mock<IUserSearcher> _userSearcher = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly GetWorkoutStatisticsQueryHandler _sut;

        public GetWorkoutStatisticsQueryHandlerTests()
        {
            _sut = new GetWorkoutStatisticsQueryHandler(
                _workoutSearcher.Object, _exerciseSearcher.Object, _userSearcher.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(new GetWorkoutStatisticsQuery(5), CancellationToken.None);

            Assert.Equal(Errors.User.Forbidden, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsNotFound()
        {
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);
            _userSearcher.Setup(s => s.ExistsAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var result = await _sut.Handle(new GetWorkoutStatisticsQuery(5), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenValid_AggregatesWorkoutsAndExercisesIntoStatistics()
        {
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);
            _userSearcher.Setup(s => s.ExistsAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _userSearcher.Setup(s => s.GetWorkoutStartDateAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(DateTime.UtcNow.AddDays(-7));

            var workoutExercise = new WorkoutExerciseDto(1, null, ExerciseId: 1, WorkoutId: 1, null, null,
                Sets: [new WorkoutSetDto(1, 5, 100, 100, 118)]);
            var workouts = new List<WorkoutDto> { new(1, 5, DateTime.UtcNow, "Push Day", [workoutExercise]) };
            _workoutSearcher.Setup(s => s.FindByUserIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(workouts);

            var exercises = new List<ExerciseDto> { new(1, "Bench Press", null, MuscleGroup.Chest, MuscleSubgroup.Chest, null) };
            _exerciseSearcher.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(exercises);

            var result = await _sut.Handle(new GetWorkoutStatisticsQuery(5), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal(1, result.Value.TotalWorkouts);
            Assert.Equal(1, result.Value.TotalSets);
            Assert.Equal("Bench Press", result.Value.FavouriteExercise);
        }
    }
}
