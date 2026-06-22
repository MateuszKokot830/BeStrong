using Application.Dto.Follower;
using Domain.Entities;

namespace Application.Mappings
{
    public static class FollowerMappings
    {
        public static FollowerDto ToDto(this Follower follower) => new(
            follower.UserId,
            follower.FollowedUserId,
            follower.FollowedAt
        );
    }
}
