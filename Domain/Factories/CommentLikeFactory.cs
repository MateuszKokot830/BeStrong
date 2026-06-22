using Domain.Entities;

namespace Domain.Factories
{
    public static class CommentLikeFactory
    {
        public static CommentLike Create(int userId, int commentId) =>
            new()
            {
                UserId = userId,
                CommentId = commentId,
                LikedAt = DateTime.UtcNow
            };
    }
}
