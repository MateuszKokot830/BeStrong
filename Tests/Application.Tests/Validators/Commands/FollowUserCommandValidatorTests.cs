using Application.Commands.Users.FollowUser;
using Application.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class FollowUserCommandValidatorTests
    {
        private readonly FollowUserCommandValidator _validator = new();

        [Fact]
        public void Validate_WithDistinctPositiveIds_HasNoErrors()
        {
            var result = _validator.TestValidate(new FollowUserCommand(UserId: 1, FollowUserId: 2));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenUserIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new FollowUserCommand(UserId: 0, FollowUserId: 2));

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_WhenFollowUserIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new FollowUserCommand(UserId: 1, FollowUserId: 0));

            result.ShouldHaveValidationErrorFor(x => x.FollowUserId);
        }

        [Fact]
        public void Validate_WhenFollowingSelf_HasError()
        {
            var result = _validator.TestValidate(new FollowUserCommand(UserId: 1, FollowUserId: 1));

            result.ShouldHaveValidationErrorFor(x => x.FollowUserId)
                .WithErrorMessage("A user cannot follow themselves.");
        }
    }
}
