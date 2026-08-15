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
        private readonly Mock<IUserSearcher> _userSearcher = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly GetWorkoutPlansQueryHandler _sut;

        public GetWorkoutPlansQueryHandlerTests()
        {
            _sut = new GetWorkoutPlansQueryHandler(_workoutPlanSearcher.Object, _userSearcher.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_ReturnsPagedPlansForTheCurrentUser()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            var criteria = new WorkoutPlanSearchCriteria();
            var plans = new PaginationList<WorkoutPlanDto>(
                [new(1, 7, [], "Plan", "Desc", WorkoutPlanCategory.FullBody, "Full Body", true, [], "Jane Doe")],
                count: 1, pageNumber: 1, pageSize: 10);
            _workoutPlanSearcher.Setup(s => s.GetPagedAsync(criteria, 7, null, It.IsAny<CancellationToken>())).ReturnsAsync(plans);

            var result = await _sut.Handle(new GetWorkoutPlansQuery(criteria), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
            _userSearcher.Verify(s => s.GetFollowedUserIdsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenOnlyFollowers_FetchesFollowedUserIdsAndPassesThemToSearcher()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            var criteria = new WorkoutPlanSearchCriteria { CreatedBy = CreatedByFilter.OnlyFollowers };
            var followedIds = new List<int> { 3, 9 };
            _userSearcher.Setup(s => s.GetFollowedUserIdsAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync(followedIds);
            var plans = new PaginationList<WorkoutPlanDto>([], count: 0, pageNumber: 1, pageSize: 10);
            _workoutPlanSearcher.Setup(s => s.GetPagedAsync(criteria, 7, followedIds, It.IsAny<CancellationToken>())).ReturnsAsync(plans);

            var result = await _sut.Handle(new GetWorkoutPlansQuery(criteria), CancellationToken.None);

            Assert.False(result.IsError);
            _workoutPlanSearcher.Verify(s => s.GetPagedAsync(criteria, 7, followedIds, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
