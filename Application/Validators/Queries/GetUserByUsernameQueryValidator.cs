using Application.Queries.Users.GetUserByUsername;
using FluentValidation;

namespace Application.Validators.Queries
{
    public sealed class GetUserByUsernameQueryValidator : AbstractValidator<GetUserByUsernameQuery>
    {
        public GetUserByUsernameQueryValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");
        }
    }
}
