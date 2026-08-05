using Domain.Factories;

namespace Domain.Tests.Factories
{
    public class CommentLikeFactoryTests
    {
        [Fact]
        public void Create_MapsUserAndCommentIds()
        {
            var like = CommentLikeFactory.Create(userId: 4, commentId: 6);

            Assert.Equal(4, like.UserId);
            Assert.Equal(6, like.CommentId);
        }

        [Fact]
        public void Create_SetsLikedAtToUtcNow()
        {
            var before = DateTime.UtcNow;

            var like = CommentLikeFactory.Create(userId: 1, commentId: 1);

            var after = DateTime.UtcNow;
            Assert.InRange(like.LikedAt, before, after);
        }
    }
}
