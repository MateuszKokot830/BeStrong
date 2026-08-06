using Application.Commands.WorkoutPlans.DeleteWorkoutPlan;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.WorkoutPlans.DeleteWorkoutPlan
{
    public class DeleteWorkoutPlanCommandHandlerTests
    {
        private readonly Mock<IWorkoutPlanRepository> _workoutPlanRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly DeleteWorkoutPlanCommandHandler _sut;

        public DeleteWorkoutPlanCommandHandlerTests()
        {
            _sut = new DeleteWorkoutPlanCommandHandler(_workoutPlanRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenPlanDoesNotExist_ReturnsNotFound()
        {
            _workoutPlanRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((WorkoutPlan?)null);

            var result = await _sut.Handle(new DeleteWorkoutPlanCommand(1), CancellationToken.None);

            Assert.Equal(Errors.WorkoutPlan.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            var plan = new WorkoutPlan { Id = 1, CreatedById = 5 };
            _workoutPlanRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(plan);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(new DeleteWorkoutPlanCommand(1), CancellationToken.None);

            Assert.Equal(Errors.WorkoutPlan.Unauthorized, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsOwnerOrAdmin_DeletesPlan()
        {
            var plan = new WorkoutPlan { Id = 1, CreatedById = 5 };
            _workoutPlanRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(plan);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new DeleteWorkoutPlanCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _workoutPlanRepository.Verify(r => r.DeleteAsync(plan, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
