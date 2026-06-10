using Application.Commands.Users.FollowUser;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class FollowUserCommandValidator : AbstractValidator<FollowUserCommand>
    {
        public FollowUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");

            RuleFor(x => x.FollowUserId)
                .GreaterThan(0).WithMessage("FollowUserId must be a valid positive integer.")
                .NotEqual(x => x.UserId).WithMessage("A user cannot follow themselves.");
        }
    }
}
