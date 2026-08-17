using Application.Dto.Workout;
using Application.Interfaces.Searchers;
using Application.Queries.Workouts.GetUserWorkouts;
using Moq;

namespace Application.Tests.Queries.Workouts.GetUserWorkouts
{
    public class GetUserWorkoutsQueryHandlerTests
    {
        private readonly Mock<IWorkoutSearcher> _workoutSearcher = new();
        private readonly GetUserWorkoutsQueryHandler _sut;

        public GetUserWorkoutsQueryHandlerTests()
        {
            _sut = new GetUserWorkoutsQueryHandler(_workoutSearcher.Object);
        }

        [Fact]
        public async Task Handle_ReturnsTheRequestedUsersWorkouts()
        {
            var workouts = new List<WorkoutDto> { new(1, 5, DateTime.UtcNow, "Push Day", []) };
            _workoutSearcher.Setup(s => s.FindByUserIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(workouts);

            var result = await _sut.Handle(new GetUserWorkoutsQuery(5), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
        }
    }
}
