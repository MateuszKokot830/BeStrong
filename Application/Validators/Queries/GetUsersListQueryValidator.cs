using Application.Queries.Users.GetUsersList;
using FluentValidation;

namespace Application.Validators.Queries
{
    public sealed class GetUsersListQueryValidator : AbstractValidator<GetUsersListQuery>
    {
        public GetUsersListQueryValidator()
        {
            RuleFor(x => x.Criteria.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.Criteria.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(50).WithMessage("Page size cannot exceed 50.");

            RuleFor(x => x.Criteria.Username)
                .MaximumLength(50).WithMessage("Username filter cannot exceed 50 characters.")
                .When(x => x.Criteria.Username is not null);
        }
    }
}
