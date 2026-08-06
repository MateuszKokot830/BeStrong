using Application.Dto.Exercise;
using Application.Interfaces.Searchers;
using Application.Queries.Workouts.GetExercises;
using Domain.Common;
using Moq;

namespace Application.Tests.Queries.Workouts.GetExercises
{
    public class GetExercisesQueryHandlerTests
    {
        private readonly Mock<IExerciseSearcher> _exerciseSearcher = new();
        private readonly GetExercisesQueryHandler _sut;

        public GetExercisesQueryHandlerTests()
        {
            _sut = new GetExercisesQueryHandler(_exerciseSearcher.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAllExercisesFromSearcher()
        {
            var exercises = new List<ExerciseDto> { new(1, "Bench Press", null, MuscleGroup.Chest, MuscleSubgroup.Chest, null) };
            _exerciseSearcher.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(exercises);

            var result = await _sut.Handle(new GetExercisesQuery(), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Single(result.Value);
        }
    }
}
