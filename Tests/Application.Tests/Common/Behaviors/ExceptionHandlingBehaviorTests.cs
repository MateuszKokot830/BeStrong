using Application.Common.Behaviors;
using Application.Tests.TestDoubles;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Common.Behaviors
{
    public class ExceptionHandlingBehaviorTests
    {
        private readonly Mock<ILogger<ExceptionHandlingBehavior<FakeCommand, ErrorOr<string>>>> _logger = new();
        private readonly ExceptionHandlingBehavior<FakeCommand, ErrorOr<string>> _sut;

        public ExceptionHandlingBehaviorTests()
        {
            _sut = new ExceptionHandlingBehavior<FakeCommand, ErrorOr<string>>(_logger.Object);
        }

        [Fact]
        public async Task Handle_WhenNextSucceeds_ReturnsItsResultUnchanged()
        {
            var result = await _sut.Handle(new FakeCommand("x"), () => Task.FromResult<ErrorOr<string>>("ok"), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal("ok", result.Value);
        }

        [Fact]
        public async Task Handle_WhenNextThrows_CatchesItAndReturnsUnexpectedError()
        {
            var result = await _sut.Handle(new FakeCommand("x"),
                () => throw new InvalidOperationException("db is down"),
                CancellationToken.None);

            Assert.True(result.IsError);
            Assert.Equal(ErrorType.Unexpected, result.FirstError.Type);
            Assert.Equal("db is down", result.FirstError.Description);
        }

        [Fact]
        public async Task Handle_WhenNextThrows_DoesNotLetTheExceptionPropagate()
        {
            var exception = await Record.ExceptionAsync(() => _sut.Handle(
                new FakeCommand("x"),
                () => throw new InvalidOperationException("db is down"),
                CancellationToken.None));

            Assert.Null(exception);
        }
    }
}
