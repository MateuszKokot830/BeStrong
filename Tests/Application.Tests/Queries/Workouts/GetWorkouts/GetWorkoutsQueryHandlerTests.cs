using Application.Dto.Workout;
using Application.Helpers;
using Application.Helpers.Criteria;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Application.Queries.Workouts.GetWorkouts;
using Moq;

namespace Application.Tests.Queries.Workouts.GetWorkouts
{
    public class GetWorkoutsQueryHandlerTests
    {
        private readonly Mock<IWorkoutSearcher> _workoutSearcher = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly GetWorkoutsQueryHandler _sut;

        public GetWorkoutsQueryHandlerTests()
        {
            _sut = new GetWorkoutsQueryHandler(_workoutSearcher.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_ReturnsPagedWorkoutsForTheCurrentUser()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            var criteria = new WorkoutSearchCriteria();
            var workouts = new PaginationList<WorkoutDto>(
                [new(1, 7, DateTime.UtcNow, "Push Day", [])],
                count: 1, pageNumber: 1, pageSize: 10);
            _workoutSearcher.Setup(s => s.GetPagedAsync(criteria, 7, It.IsAny<CancellationToken>())).ReturnsAsync(workouts);

            var result = await _sut.Handle(new GetWorkoutsQuery(criteria), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
        }
    }
}
