using Application.Common.Behaviors;
using Application.Tests.TestDoubles;
using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Common.Behaviors
{
    public class ValidationBehaviorTests
    {
        private readonly Mock<ILogger<ValidationBehavior<FakeCommand, ErrorOr<string>>>> _logger = new();

        private static Mock<IValidator<FakeCommand>> ValidatorReturning(params ValidationFailure[] failures)
        {
            var mock = new Mock<IValidator<FakeCommand>>();
            mock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<FakeCommand>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));
            return mock;
        }

        [Fact]
        public async Task Handle_WithNoValidatorsRegistered_CallsNextDirectly()
        {
            var sut = new ValidationBehavior<FakeCommand, ErrorOr<string>>([], _logger.Object);
            var nextCalled = false;

            var result = await sut.Handle(new FakeCommand("x"), () =>
            {
                nextCalled = true;
                return Task.FromResult<ErrorOr<string>>("ok");
            }, CancellationToken.None);

            Assert.True(nextCalled);
            Assert.Equal("ok", result.Value);
        }

        [Fact]
        public async Task Handle_WhenAllValidatorsPass_CallsNext()
        {
            var validator = ValidatorReturning();
            var sut = new ValidationBehavior<FakeCommand, ErrorOr<string>>([validator.Object], _logger.Object);

            var result = await sut.Handle(new FakeCommand("x"), () => Task.FromResult<ErrorOr<string>>("ok"), CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal("ok", result.Value);
        }

        [Fact]
        public async Task Handle_WhenValidationFails_ReturnsValidationErrorsWithoutCallingNext()
        {
            var validator = ValidatorReturning(new ValidationFailure("Value", "Value is required."));
            var sut = new ValidationBehavior<FakeCommand, ErrorOr<string>>([validator.Object], _logger.Object);
            var nextCalled = false;

            var result = await sut.Handle(new FakeCommand(""), () =>
            {
                nextCalled = true;
                return Task.FromResult<ErrorOr<string>>("ok");
            }, CancellationToken.None);

            Assert.False(nextCalled);
            Assert.True(result.IsError);
            Assert.Equal(ErrorType.Validation, result.FirstError.Type);
            Assert.Equal("Value", result.FirstError.Code);
            Assert.Equal("Value is required.", result.FirstError.Description);
        }

        [Fact]
        public async Task Handle_WhenMultipleValidatorsFail_AggregatesFailuresFromAll()
        {
            var validator1 = ValidatorReturning(new ValidationFailure("A", "A is invalid."));
            var validator2 = ValidatorReturning(new ValidationFailure("B", "B is invalid."));
            var sut = new ValidationBehavior<FakeCommand, ErrorOr<string>>([validator1.Object, validator2.Object], _logger.Object);

            var result = await sut.Handle(new FakeCommand(""), () => Task.FromResult<ErrorOr<string>>("ok"), CancellationToken.None);

            Assert.True(result.IsError);
            Assert.Equal(2, result.Errors.Count);
        }
    }
}
