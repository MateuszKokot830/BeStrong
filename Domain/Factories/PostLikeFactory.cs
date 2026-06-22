using Domain.Entities;

namespace Domain.Factories
{
    public static class PostLikeFactory
    {
        public static PostLike Create(int userId, int postId) =>
            new()
            {
                UserId = userId,
                PostId = postId,
                LikedAt = DateTime.UtcNow
            };
    }
}
