using Domain.Entities;

namespace Domain.Factories
{
    public static class CommentFactory
    {
        public static Comment Create(int userId, string? description, int postId) =>
            new()
            {
                UserId = userId,
                Description = description,
                CreatedDate = DateTime.UtcNow,
                PostId = postId
            };
    }
}
