using Application.Common.Behaviors;
using Application.Tests.TestDoubles;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Common.Behaviors
{
    public class LoggingBehaviorTests
    {
        private readonly Mock<ILogger<LoggingBehavior<FakeCommand, ErrorOr<string>>>> _logger = new();
        private readonly LoggingBehavior<FakeCommand, ErrorOr<string>> _sut;

        public LoggingBehaviorTests()
        {
            _sut = new LoggingBehavior<FakeCommand, ErrorOr<string>>(_logger.Object);
        }

        [Fact]
        public async Task Handle_ReturnsNextsResultUnchanged()
        {
            var result = await _sut.Handle(new FakeCommand("x"), () => Task.FromResult<ErrorOr<string>>("ok"), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal("ok", result.Value);
        }

        [Fact]
        public async Task Handle_CallsNextExactlyOnce()
        {
            var callCount = 0;

            await _sut.Handle(new FakeCommand("x"), () =>
            {
                callCount++;
                return Task.FromResult<ErrorOr<string>>("ok");
            }, CancellationToken.None);

            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task Handle_WhenNextThrows_LetsTheExceptionPropagate()
        {
            // LoggingBehavior has no try/catch, unlike ExceptionHandlingBehavior — it must run
            // after ExceptionHandlingBehavior in the pipeline, or exceptions here go unhandled.
            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Handle(
                new FakeCommand("x"),
                () => throw new InvalidOperationException("boom"),
                CancellationToken.None));
        }
    }
}
