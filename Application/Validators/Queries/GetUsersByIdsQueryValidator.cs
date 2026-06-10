using Application.Queries.Users.GetUsersByIds;
using FluentValidation;

namespace Application.Validators.Queries
{
    public sealed class GetUsersByIdsQueryValidator : AbstractValidator<GetUsersByIdsQuery>
    {
        public GetUsersByIdsQueryValidator()
        {
            RuleFor(x => x.UserIds)
                .NotEmpty().WithMessage("At least one UserId must be provided.");

            RuleForEach(x => x.UserIds)
                .GreaterThan(0).WithMessage("Each UserId must be a valid positive integer.");
        }
    }
}
