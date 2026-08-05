using Domain.Aggregates;
using Domain.Factories;

namespace Domain.Tests.Factories
{
    public class FollowerFactoryTests
    {
        [Fact]
        public void Create_MapsIdsAndNavigationPropertiesFromBothUsers()
        {
            var user = new User { Id = 1, UserName = "follower" };
            var followedUser = new User { Id = 2, UserName = "followed" };

            var follower = FollowerFactory.Create(user, followedUser);

            Assert.Equal(1, follower.UserId);
            Assert.Same(user, follower.User);
            Assert.Equal(2, follower.FollowedUserId);
            Assert.Same(followedUser, follower.FollowedUser);
        }

        [Fact]
        public void Create_SetsFollowedAtToUtcNow()
        {
            var before = DateTime.UtcNow;

            var follower = FollowerFactory.Create(new User { Id = 1 }, new User { Id = 2 });

            var after = DateTime.UtcNow;
            Assert.InRange(follower.FollowedAt, before, after);
        }
    }
}
