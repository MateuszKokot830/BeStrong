using Application.Queries.Login;
using FluentValidation;

namespace Application.Validators.Queries
{
    public sealed class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(x => x.UserLoginRequestDto.UserName)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.UserLoginRequestDto.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
