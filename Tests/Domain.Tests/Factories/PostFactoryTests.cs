using Domain.Common;
using Domain.Factories;

namespace Domain.Tests.Factories
{
    public class PostFactoryTests
    {
        [Fact]
        public void Create_MapsAllProvidedFields()
        {
            var post = PostFactory.Create(userId: 7, PostType.WorkoutPublication, description: "Leg day", workoutId: 3, workoutPlanId: 9);

            Assert.Equal(7, post.UserId);
            Assert.Equal(PostType.WorkoutPublication, post.Type);
            Assert.Equal("Leg day", post.Description);
            Assert.Equal(3, post.WorkoutId);
            Assert.Equal(9, post.WorkoutPlanId);
        }

        [Fact]
        public void Create_SetsCreatedDateToUtcNow()
        {
            var before = DateTime.UtcNow;

            var post = PostFactory.Create(userId: 1, PostType.Normal, description: null, workoutId: null, workoutPlanId: null);

            var after = DateTime.UtcNow;
            Assert.InRange(post.CreatedDate, before, after);
        }

        [Fact]
        public void Create_WithNullWorkoutAndPlanIds_IsAllowed()
        {
            var post = PostFactory.Create(userId: 1, PostType.Normal, description: "hello", workoutId: null, workoutPlanId: null);

            Assert.Null(post.WorkoutId);
            Assert.Null(post.WorkoutPlanId);
        }
    }
}
