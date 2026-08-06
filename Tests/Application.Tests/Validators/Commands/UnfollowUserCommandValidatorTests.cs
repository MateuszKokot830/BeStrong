using Application.Commands.Users.UnfollowUser;
using Application.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class UnfollowUserCommandValidatorTests
    {
        private readonly UnfollowUserCommandValidator _validator = new();

        [Fact]
        public void Validate_WithDistinctPositiveIds_HasNoErrors()
        {
            var result = _validator.TestValidate(new UnfollowUserCommand(UserId: 1, UnfollowUserId: 2));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenUnfollowingSelf_HasError()
        {
            var result = _validator.TestValidate(new UnfollowUserCommand(UserId: 1, UnfollowUserId: 1));

            result.ShouldHaveValidationErrorFor(x => x.UnfollowUserId)
                .WithErrorMessage("A user cannot unfollow themselves.");
        }
    }
}
