using Domain.Aggregates;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Searchers;
using Infrastructure.Tests.TestDoubles;

namespace Infrastructure.Tests.Searchers
{
    public class PostSearcherTests : SqliteInMemoryFixture
    {
        private readonly PostSearcher _sut;

        public PostSearcherTests()
        {
            _sut = new PostSearcher(Context);
        }

        [Fact]
        public async Task FindByIdAsync_WhenPostDoesNotExist_ReturnsNull()
        {
            var result = await _sut.FindByIdAsync(999, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task FindByIdAsync_MapsLikesCountAndComments()
        {
            var user = await CreateUserAsync();
            var post = new Post { UserId = user.Id, Type = PostType.Normal, Description = "hi", CreatedDate = DateTime.UtcNow };
            Context.Posts.Add(post);
            await Context.SaveChangesAsync();
            Context.PostLikes.Add(new PostLike { UserId = user.Id, PostId = post.Id });
            Context.Comments.Add(new Comment { UserId = user.Id, PostId = post.Id, Description = "nice", CreatedDate = DateTime.UtcNow });
            await Context.SaveChangesAsync();

            var result = await _sut.FindByIdAsync(post.Id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(1, result!.LikesCount);
            Assert.Single(result.Comments);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsPostsOrderedByCreatedDateAscending()
        {
            var user = await CreateUserAsync();
            var older = new Post { UserId = user.Id, Type = PostType.Normal, Description = "older", CreatedDate = DateTime.UtcNow.AddDays(-1) };
            var newer = new Post { UserId = user.Id, Type = PostType.Normal, Description = "newer", CreatedDate = DateTime.UtcNow };
            Context.Posts.AddRange(newer, older);
            await Context.SaveChangesAsync();

            var result = await _sut.GetAllAsync(CancellationToken.None);

            Assert.Equal(["older", "newer"], result.Select(p => p.Description));
        }

        [Fact]
        public async Task FindByUserIdAsync_OnlyReturnsThatUsersPosts()
        {
            var user1 = await CreateUserAsync("alice");
            var user2 = await CreateUserAsync("bob");
            Context.Posts.AddRange(
                new Post { UserId = user1.Id, Type = PostType.Normal, Description = "mine", CreatedDate = DateTime.UtcNow },
                new Post { UserId = user2.Id, Type = PostType.Normal, Description = "not mine", CreatedDate = DateTime.UtcNow });
            await Context.SaveChangesAsync();

            var result = await _sut.FindByUserIdAsync(user1.Id, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("mine", result[0].Description);
        }

        [Fact]
        public async Task FindByIdAsync_ForAWorkoutPublicationPost_IncludesTheWorkoutExercisesAndSets()
        {
            var user = await CreateUserAsync();
            var exercise = await CreateExerciseAsync();
            var workout = new Workout
            {
                UserId = user.Id,
                Name = "Push Day",
                Date = DateTime.UtcNow,
                WorkoutExercises = [new WorkoutExercise { ExerciseId = exercise.Id, Sets = [new WorkoutSet { SetNumber = 1, Reps = 10 }] }]
            };
            Context.Workouts.Add(workout);
            await Context.SaveChangesAsync();
            var post = new Post
            {
                UserId = user.Id,
                Type = PostType.WorkoutPublication,
                Description = "Push Day",
                CreatedDate = DateTime.UtcNow,
                WorkoutId = workout.Id
            };
            Context.Posts.Add(post);
            await Context.SaveChangesAsync();

            var result = await _sut.FindByIdAsync(post.Id, CancellationToken.None);

            Assert.NotNull(result!.Workout);
            Assert.Single(result.Workout!.WorkoutExercises);
            Assert.Single(result.Workout.WorkoutExercises.First().Sets);
        }

        [Fact]
        public async Task FindByUserIdsAsync_ReturnsPostsFromAnyOfTheGivenUsers_OrderedByCreatedDateDescending()
        {
            var user1 = await CreateUserAsync("alice");
            var user2 = await CreateUserAsync("bob");
            var user3 = await CreateUserAsync("carol");
            Context.Posts.AddRange(
                new Post { UserId = user1.Id, Type = PostType.Normal, Description = "older", CreatedDate = DateTime.UtcNow.AddDays(-1) },
                new Post { UserId = user2.Id, Type = PostType.Normal, Description = "newer", CreatedDate = DateTime.UtcNow },
                new Post { UserId = user3.Id, Type = PostType.Normal, Description = "excluded", CreatedDate = DateTime.UtcNow });
            await Context.SaveChangesAsync();

            var result = await _sut.FindByUserIdsAsync([user1.Id, user2.Id], CancellationToken.None);

            Assert.Equal(["newer", "older"], result.Select(p => p.Description));
        }
    }
}
