using Application.Commands.WorkoutPlans.CreateWorkoutPlan;
using Application.Dto.WorkoutPlan;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Common;
using Moq;

namespace Application.Tests.Commands.WorkoutPlans.CreateWorkoutPlan
{
    public class CreateWorkoutPlanCommandHandlerTests
    {
        private readonly Mock<IWorkoutPlanRepository> _workoutPlanRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly CreateWorkoutPlanCommandHandler _sut;

        public CreateWorkoutPlanCommandHandlerTests()
        {
            _sut = new CreateWorkoutPlanCommandHandler(_workoutPlanRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_CreatesPlanOwnedByCurrentUserAndPersistsIt()
        {
            _currentUserService.Setup(s => s.UserId).Returns(7);
            var dto = new WorkoutPlanCreateDto("Push Pull Legs", "desc", WorkoutPlanCategory.PushPullLegs, IsPublic: true, []);

            var result = await _sut.Handle(new CreateWorkoutPlanCommand(dto), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal(7, result.Value.CreatedById);
            Assert.Equal("Push Pull Legs", result.Value.Name);
            Assert.True(result.Value.IsPublic);
            _workoutPlanRepository.Verify(r => r.AddAsync(
                It.Is<WorkoutPlan>(p => p.CreatedById == 7 && p.Name == "Push Pull Legs"),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
