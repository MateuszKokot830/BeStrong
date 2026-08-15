using Application.Commands.WorkoutPlans.AssignWorkoutPlan;
using Application.Dto.WorkoutPlan;
using Application.Interfaces.Repositories;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Common;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.WorkoutPlans.AssignWorkoutPlan
{
    public class AssignWorkoutPlanCommandHandlerTests
    {
        private readonly Mock<IWorkoutPlanSearcher> _workoutPlanSearcher = new();
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly AssignWorkoutPlanCommandHandler _sut;

        public AssignWorkoutPlanCommandHandlerTests()
        {
            _sut = new AssignWorkoutPlanCommandHandler(_workoutPlanSearcher.Object, _userRepository.Object, _currentUserService.Object);
        }

        private static WorkoutPlanDto Plan(int id, int createdById, bool isPublic) =>
            new(id, createdById, [], "Plan", "Desc", WorkoutPlanCategory.FullBody, "Full Body", isPublic, [], "Jane Doe");

        [Fact]
        public async Task Handle_WhenPlanDoesNotExist_ReturnsNotFound()
        {
            _workoutPlanSearcher.Setup(s => s.FindByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((WorkoutPlanDto?)null);

            var result = await _sut.Handle(new AssignWorkoutPlanCommand(1), CancellationToken.None);

            Assert.Equal(Errors.WorkoutPlan.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenPlanIsPrivateAndCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            _workoutPlanSearcher.Setup(s => s.FindByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Plan(1, createdById: 7, isPublic: false));
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(7)).Returns(false);

            var result = await _sut.Handle(new AssignWorkoutPlanCommand(1), CancellationToken.None);

            Assert.Equal(Errors.WorkoutPlan.Unauthorized, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenPlanIsPublic_SkipsOwnershipCheckEvenForAStranger()
        {
            _workoutPlanSearcher.Setup(s => s.FindByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Plan(1, createdById: 7, isPublic: true));
            _currentUserService.Setup(s => s.UserId).Returns(99);
            _userRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync(new User { Id = 99 });

            var result = await _sut.Handle(new AssignWorkoutPlanCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            _currentUserService.Verify(s => s.IsOwnerOrAdmin(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenCurrentUserDoesNotExist_ReturnsUserNotFound()
        {
            _workoutPlanSearcher.Setup(s => s.FindByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Plan(1, createdById: 7, isPublic: true));
            _currentUserService.Setup(s => s.UserId).Returns(99);
            _userRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var result = await _sut.Handle(new AssignWorkoutPlanCommand(1), CancellationToken.None);

            Assert.Equal(Errors.User.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenValid_AssignsPlanToCurrentUser()
        {
            var user = new User { Id = 99 };
            _workoutPlanSearcher.Setup(s => s.FindByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Plan(1, createdById: 99, isPublic: false));
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(99)).Returns(true);
            _currentUserService.Setup(s => s.UserId).Returns(99);
            _userRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _sut.Handle(new AssignWorkoutPlanCommand(1), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal(1, user.WorkoutPlanId);
            _userRepository.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
