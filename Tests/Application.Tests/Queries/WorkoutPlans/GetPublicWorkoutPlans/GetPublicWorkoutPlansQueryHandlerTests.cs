using Application.Dto.WorkoutPlan;
using Application.Interfaces.Searchers;
using Application.Queries.WorkoutPlans.GetPublicWorkoutPlans;
using Domain.Common;
using Moq;

namespace Application.Tests.Queries.WorkoutPlans.GetPublicWorkoutPlans
{
    public class GetPublicWorkoutPlansQueryHandlerTests
    {
        private readonly Mock<IWorkoutPlanSearcher> _workoutPlanSearcher = new();
        private readonly GetPublicWorkoutPlansQueryHandler _sut;

        public GetPublicWorkoutPlansQueryHandlerTests()
        {
            _sut = new GetPublicWorkoutPlansQueryHandler(_workoutPlanSearcher.Object);
        }

        [Fact]
        public async Task Handle_ReturnsPublicPlansFromSearcher()
        {
            var plans = new List<WorkoutPlanDto> { new(1, 7, [], "Plan", "Desc", WorkoutPlanCategory.FullBody, true, []) };
            _workoutPlanSearcher.Setup(s => s.GetPublicAsync(It.IsAny<CancellationToken>())).ReturnsAsync(plans);

            var result = await _sut.Handle(new GetPublicWorkoutPlansQuery(), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
        }
    }
}
