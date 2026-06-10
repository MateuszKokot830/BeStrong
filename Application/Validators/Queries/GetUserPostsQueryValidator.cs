using Application.Queries.Posts.GetUserPosts;
using FluentValidation;

namespace Application.Validators.Queries
{
    public sealed class GetUserPostsQueryValidator : AbstractValidator<GetUserPostsQuery>
    {
        public GetUserPostsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");
        }
    }
}
