using Application.Dto.WorkoutPlan;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Application.Queries.WorkoutPlans.GetWorkoutPlanById;
using Domain.Common;
using Domain.Errors;
using Moq;

namespace Application.Tests.Queries.WorkoutPlans.GetWorkoutPlanById
{
    public class GetWorkoutPlanByIdQueryHandlerTests
    {
        private readonly Mock<IWorkoutPlanSearcher> _workoutPlanSearcher = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly GetWorkoutPlanByIdQueryHandler _sut;

        public GetWorkoutPlanByIdQueryHandlerTests()
        {
            _sut = new GetWorkoutPlanByIdQueryHandler(_workoutPlanSearcher.Object, _currentUserService.Object);
        }

        private static WorkoutPlanDto Plan(int id, int createdById, bool isPublic) =>
            new(id, createdById, [], "Plan", "Desc", WorkoutPlanCategory.FullBody, "Full Body", isPublic, []);

        [Fact]
        public async Task Handle_WhenPlanDoesNotExist_ReturnsNotFound()
        {
            _workoutPlanSearcher.Setup(s => s.FindByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((WorkoutPlanDto?)null);

            var result = await _sut.Handle(new GetWorkoutPlanByIdQuery(1), CancellationToken.None);

            Assert.Equal(Errors.WorkoutPlan.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenPlanIsPrivateAndCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            _workoutPlanSearcher.Setup(s => s.FindByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Plan(1, createdById: 7, isPublic: false));
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(7)).Returns(false);

            var result = await _sut.Handle(new GetWorkoutPlanByIdQuery(1), CancellationToken.None);

            Assert.Equal(Errors.WorkoutPlan.Unauthorized, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenPlanIsPublic_IsVisibleEvenToAStranger()
        {
            var plan = Plan(1, createdById: 7, isPublic: true);
            _workoutPlanSearcher.Setup(s => s.FindByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(plan);

            var result = await _sut.Handle(new GetWorkoutPlanByIdQuery(1), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Same(plan, result.Value);
            _currentUserService.Verify(s => s.IsOwnerOrAdmin(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenPlanIsPrivateAndCallerIsOwner_IsVisible()
        {
            var plan = Plan(1, createdById: 7, isPublic: false);
            _workoutPlanSearcher.Setup(s => s.FindByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(plan);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(7)).Returns(true);

            var result = await _sut.Handle(new GetWorkoutPlanByIdQuery(1), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Same(plan, result.Value);
        }
    }
}
