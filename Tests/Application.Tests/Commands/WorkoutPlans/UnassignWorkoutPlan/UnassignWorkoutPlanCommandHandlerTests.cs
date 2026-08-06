using Application.Commands.WorkoutPlans.UnassignWorkoutPlan;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.WorkoutPlans.UnassignWorkoutPlan
{
    public class UnassignWorkoutPlanCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly UnassignWorkoutPlanCommandHandler _sut;

        public UnassignWorkoutPlanCommandHandlerTests()
        {
            _sut = new UnassignWorkoutPlanCommandHandler(_userRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_WhenCurrentUserDoesNotExist_ReturnsNotFound()
        {
            _currentUserService.Setup(s => s.UserId).Returns(1);
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var result = await _sut.Handle(new UnassignWorkoutPlanCommand(5), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenRequestedPlanIsNotTheUsersCurrentPlan_LeavesAssignmentUnchanged()
        {
            var user = new User { Id = 1, WorkoutPlanId = 5 };
            _currentUserService.Setup(s => s.UserId).Returns(1);
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _sut.Handle(new UnassignWorkoutPlanCommand(999), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal(5, user.WorkoutPlanId);
            _userRepository.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRequestedPlanIsTheUsersCurrentPlan_ClearsIt()
        {
            var user = new User { Id = 1, WorkoutPlanId = 5 };
            _currentUserService.Setup(s => s.UserId).Returns(1);
            _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _sut.Handle(new UnassignWorkoutPlanCommand(5), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Null(user.WorkoutPlanId);
            _userRepository.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
