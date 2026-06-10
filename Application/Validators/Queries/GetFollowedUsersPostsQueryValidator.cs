using Application.Queries.Posts.GetFollowedUsersPosts;
using FluentValidation;

namespace Application.Validators.Queries
{
    public sealed class GetFollowedUsersPostsQueryValidator : AbstractValidator<GetFollowedUsersPostsQuery>
    {
        public GetFollowedUsersPostsQueryValidator()
        {
            RuleFor(x => x.FollowersIds)
                .NotEmpty().WithMessage("At least one follower ID must be provided.");

            RuleForEach(x => x.FollowersIds)
                .GreaterThan(0).WithMessage("Each follower ID must be a valid positive integer.");
        }
    }
}
