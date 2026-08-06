using Application.Queries.Workouts.GetOneRepMax;

namespace Application.Tests.Queries.Workouts.GetOneRepMax
{
    public class GetOneRepMaxQueryHandlerTests
    {
        private readonly GetOneRepMaxQueryHandler _sut = new();

        [Fact]
        public async Task Handle_DelegatesToOneRepMaxCalculator()
        {
            var result = await _sut.Handle(new GetOneRepMaxQuery(Weight: 100, Reps: 5), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal(113, result.Value);
        }
    }
}
