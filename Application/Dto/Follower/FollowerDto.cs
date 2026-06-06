namespace Application.Dto.Follower
{
    public record FollowerDto(
        int UserId,
        int FollowedUserId
    );
}