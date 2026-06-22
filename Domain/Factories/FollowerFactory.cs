using Domain.Aggregates;
using Domain.Entities;

namespace Domain.Factories
{
    public static class FollowerFactory
    {
        public static Follower Create(User user, User followedUser) =>
            new()
            {
                UserId = user.Id,
                User = user,
                FollowedUserId = followedUser.Id,
                FollowedUser = followedUser,
                FollowedAt = DateTime.UtcNow
            };
    }
}
