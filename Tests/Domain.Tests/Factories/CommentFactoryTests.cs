using Domain.Factories;

namespace Domain.Tests.Factories
{
    public class CommentFactoryTests
    {
        [Fact]
        public void Create_MapsUserPostAndDescription()
        {
            var comment = CommentFactory.Create(userId: 3, description: "Nice work!", postId: 8);

            Assert.Equal(3, comment.UserId);
            Assert.Equal(8, comment.PostId);
            Assert.Equal("Nice work!", comment.Description);
        }

        [Fact]
        public void Create_SetsCreatedDateToUtcNow()
        {
            var before = DateTime.UtcNow;

            var comment = CommentFactory.Create(userId: 1, description: null, postId: 1);

            var after = DateTime.UtcNow;
            Assert.InRange(comment.CreatedDate, before, after);
        }
    }
}
