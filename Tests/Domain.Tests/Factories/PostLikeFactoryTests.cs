using Domain.Factories;

namespace Domain.Tests.Factories
{
    public class PostLikeFactoryTests
    {
        [Fact]
        public void Create_MapsUserAndPostIds()
        {
            var like = PostLikeFactory.Create(userId: 2, postId: 5);

            Assert.Equal(2, like.UserId);
            Assert.Equal(5, like.PostId);
        }

        [Fact]
        public void Create_SetsLikedAtToUtcNow()
        {
            var before = DateTime.UtcNow;

            var like = PostLikeFactory.Create(userId: 1, postId: 1);

            var after = DateTime.UtcNow;
            Assert.InRange(like.LikedAt, before, after);
        }
    }
}
