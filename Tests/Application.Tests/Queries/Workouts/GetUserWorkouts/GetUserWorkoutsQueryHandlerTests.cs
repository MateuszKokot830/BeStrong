using Application.Dto.Workout;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Application.Queries.Workouts.GetUserWorkouts;
using Domain.Errors;
using Moq;

namespace Application.Tests.Queries.Workouts.GetUserWorkouts
{
    public class GetUserWorkoutsQueryHandlerTests
    {
        private readonly Mock<IWorkoutSearcher> _workoutSearcher = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly GetUserWorkoutsQueryHandler _sut;

        public GetUserWorkoutsQueryHandlerTests()
        {
            _sut = new GetUserWorkoutsQueryHandler(_workoutSearcher.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(new GetUserWorkoutsQuery(5), CancellationToken.None);

            Assert.Equal(Errors.User.Unauthorized, result.FirstError);
            _workoutSearcher.Verify(s => s.FindByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenCallerIsOwnerOrAdmin_ReturnsTheirWorkouts()
        {
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);
            var workouts = new List<WorkoutDto> { new(1, 5, DateTime.UtcNow, "Push Day", []) };
            _workoutSearcher.Setup(s => s.FindByUserIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(workouts);

            var result = await _sut.Handle(new GetUserWorkoutsQuery(5), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
        }
    }
}
