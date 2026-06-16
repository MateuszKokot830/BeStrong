using Application.Commands.Users.UnfollowUser;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class UnfollowUserCommandValidator : AbstractValidator<UnfollowUserCommand>
    {
        public UnfollowUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");

            RuleFor(x => x.UnfollowUserId)
                .GreaterThan(0).WithMessage("UnfollowUserId must be a valid positive integer.")
                .NotEqual(x => x.UserId).WithMessage("A user cannot unfollow themselves.");
        }
    }
}
