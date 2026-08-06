using Application.Common.Behaviors;
using Application.Interfaces.Common;
using Application.Tests.TestDoubles;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Common.Behaviors
{
    public class TransactionBehaviorTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        private TransactionBehavior<TRequest, ErrorOr<string>> CreateSut<TRequest>()
            where TRequest : MediatR.IRequest<ErrorOr<string>> =>
            new(_unitOfWork.Object, Mock.Of<ILogger<TransactionBehavior<TRequest, ErrorOr<string>>>>());

        [Fact]
        public async Task Handle_ForARequestNamedCommand_BeginsAndCommitsATransaction()
        {
            _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Mock.Of<IAsyncDisposable>());
            var sut = CreateSut<FakeCommand>();

            var result = await sut.Handle(new FakeCommand("x"), () => Task.FromResult<ErrorOr<string>>("ok"), CancellationToken.None);

            Assert.False(result.IsError);
            _unitOfWork.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ForARequestNotNamedCommand_SkipsTheTransactionEntirely()
        {
            // IsNotCommand() is decided purely by whether the type name ends with "Command" —
            // a query-shaped request that happens to be named "...Command" would get wrapped
            // in a transaction it doesn't need, and vice versa. Documents that naming convention.
            var sut = CreateSut<FakeQuery>();

            var result = await sut.Handle(new FakeQuery("x"), () => Task.FromResult<ErrorOr<string>>("ok"), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal("ok", result.Value);
            _unitOfWork.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenNextThrows_RollsBackAndRethrowsWithoutCommitting()
        {
            _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Mock.Of<IAsyncDisposable>());
            var sut = CreateSut<FakeCommand>();

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(
                new FakeCommand("x"),
                () => throw new InvalidOperationException("db is down"),
                CancellationToken.None));

            _unitOfWork.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
