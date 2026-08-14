using Application.Dto.WorkoutPlan;
using Application.Helpers;
using Application.Helpers.Criteria;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Application.Queries.WorkoutPlans.GetWorkoutPlans;
using Domain.Common;
using Moq;

namespace Application.Tests.Queries.WorkoutPlans.GetWorkoutPlans
{
    public class GetWorkoutPlansQueryHandlerTests
    {
        private readonly Mock<IWorkoutPlanSearcher> _workoutPlanSearcher = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly GetWorkoutPlansQueryHandler _sut;

        public GetWorkoutPlansQueryHandlerTests()
        {
            _sut = new GetWorkoutPlansQueryHandler(_workoutPlanSearcher.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_ReturnsPagedPlansForTheCurrentUser()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            var criteria = new WorkoutPlanSearchCriteria();
            var plans = new PaginationList<WorkoutPlanDto>(
                [new(1, 7, [], "Plan", "Desc", WorkoutPlanCategory.FullBody, "Full Body", true, [])],
                count: 1, pageNumber: 1, pageSize: 10);
            _workoutPlanSearcher.Setup(s => s.GetPagedAsync(criteria, 7, It.IsAny<CancellationToken>())).ReturnsAsync(plans);

            var result = await _sut.Handle(new GetWorkoutPlansQuery(criteria), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
        }
    }
}
