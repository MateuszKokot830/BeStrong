using Application.Commands.WorkoutPlans.UpdateWorkoutPlan;
using Application.Dto.WorkoutPlan;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Aggregates;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;
using Moq;

namespace Application.Tests.Commands.WorkoutPlans.UpdateWorkoutPlan
{
    public class UpdateWorkoutPlanCommandHandlerTests
    {
        private readonly Mock<IWorkoutPlanRepository> _workoutPlanRepository = new();
        private readonly Mock<ICurrentUserService> _currentUserService = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly UpdateWorkoutPlanCommandHandler _sut;

        public UpdateWorkoutPlanCommandHandlerTests()
        {
            _sut = new UpdateWorkoutPlanCommandHandler(_workoutPlanRepository.Object, _currentUserService.Object, _unitOfWork.Object);
        }

        private static WorkoutPlanCreateDto ValidDto() => new(
            "Updated Name", "Updated desc", WorkoutPlanCategory.PushPull, IsPublic: true,
            [new WorkoutTemplateCreateDto(0, "Day A", [new WorkoutTemplateExerciseCreateDto(0, 1, Sets: 3, MinReps: 8, MaxReps: 10)])]);

        [Fact]
        public async Task Handle_WhenPlanDoesNotExist_ReturnsNotFound()
        {
            _workoutPlanRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((WorkoutPlan?)null);

            var result = await _sut.Handle(new UpdateWorkoutPlanCommand(1, ValidDto()), CancellationToken.None);

            Assert.Equal(Errors.WorkoutPlan.NotFound, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenCallerIsNotOwnerOrAdmin_ReturnsUnauthorized()
        {
            var plan = new WorkoutPlan { Id = 1, CreatedById = 5 };
            _workoutPlanRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(plan);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(false);

            var result = await _sut.Handle(new UpdateWorkoutPlanCommand(1, ValidDto()), CancellationToken.None);

            Assert.Equal(Errors.WorkoutPlan.Unauthorized, result.FirstError);
        }

        [Fact]
        public async Task Handle_WhenPlanIsInUse_ReturnsInUse()
        {
            var plan = new WorkoutPlan { Id = 1, CreatedById = 5, UsedBy = [new User { Id = 99 }] };
            _workoutPlanRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(plan);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new UpdateWorkoutPlanCommand(1, ValidDto()), CancellationToken.None);

            Assert.Equal(Errors.WorkoutPlan.InUse, result.FirstError);
            _workoutPlanRepository.Verify(r => r.UpdateAsync(It.IsAny<WorkoutPlan>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenValid_UpdatesFieldsAndReplacesTemplates()
        {
            var plan = new WorkoutPlan
            {
                Id = 1,
                CreatedById = 5,
                Name = "Old Name",
                Category = WorkoutPlanCategory.FullBody,
                IsPublic = false,
                WorkoutTemplates = [new WorkoutTemplate { Name = "Old Template" }]
            };
            var savedPlan = new WorkoutPlan
            {
                Id = 1,
                CreatedById = 5,
                Name = "Updated Name",
                Category = WorkoutPlanCategory.PushPull,
                IsPublic = true,
                WorkoutTemplates =
                [
                    new WorkoutTemplate
                    {
                        Name = "Day A",
                        Exercises = [new WorkoutTemplateExercise { ExerciseId = 1, Exercise = new Exercise { Id = 1, Name = "Bench Press" }, Sets = 3, MinReps = 8, MaxReps = 10 }]
                    }
                ]
            };
            _workoutPlanRepository.SetupSequence(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(plan)
                .ReturnsAsync(savedPlan);
            _currentUserService.Setup(s => s.IsOwnerOrAdmin(5)).Returns(true);

            var result = await _sut.Handle(new UpdateWorkoutPlanCommand(1, ValidDto()), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal("Updated Name", result.Value.Name);
            Assert.True(result.Value.IsPublic);
            Assert.Single(plan.WorkoutTemplates);
            Assert.Equal("Day A", plan.WorkoutTemplates.First().Name);
            _workoutPlanRepository.Verify(r => r.UpdateAsync(plan, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
