using Domain.Aggregates;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Tests.TestDoubles;

namespace Infrastructure.Tests.Repositories
{
    public class CommentRepositoryTests : SqliteInMemoryFixture
    {
        private readonly CommentRepository _sut;

        public CommentRepositoryTests()
        {
            _sut = new CommentRepository(Context);
        }

        private async Task<Post> CreatePostAsync(int userId)
        {
            var post = new Post { UserId = userId, Type = PostType.Normal, Description = "hi", CreatedDate = DateTime.UtcNow };
            Context.Posts.Add(post);
            await Context.SaveChangesAsync();
            return post;
        }

        [Fact]
        public async Task AddAsync_ThenSaveChanges_PersistsTheComment()
        {
            var user = await CreateUserAsync();
            var post = await CreatePostAsync(user.Id);
            var comment = new Comment { UserId = user.Id, PostId = post.Id, Description = "nice", CreatedDate = DateTime.UtcNow };

            await _sut.AddAsync(comment, CancellationToken.None);
            await Context.SaveChangesAsync();

            var loaded = await _sut.GetByIdAsync(comment.Id, CancellationToken.None);
            Assert.NotNull(loaded);
            Assert.Equal("nice", loaded!.Description);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCommentDoesNotExist_ReturnsNull()
        {
            var loaded = await _sut.GetByIdAsync(999, CancellationToken.None);

            Assert.Null(loaded);
        }

        [Fact]
        public async Task GetByIdAsync_IncludesLikes()
        {
            var user = await CreateUserAsync();
            var post = await CreatePostAsync(user.Id);
            var comment = new Comment { UserId = user.Id, PostId = post.Id, Description = "nice", CreatedDate = DateTime.UtcNow };
            await _sut.AddAsync(comment, CancellationToken.None);
            await Context.SaveChangesAsync();
            await _sut.AddLikeAsync(new CommentLike { UserId = user.Id, CommentId = comment.Id }, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var loaded = await _sut.GetByIdAsync(comment.Id, CancellationToken.None);

            Assert.Single(loaded!.Likes);
        }

        [Fact]
        public async Task UpdateAsync_ThenSaveChanges_PersistsChanges()
        {
            var user = await CreateUserAsync();
            var post = await CreatePostAsync(user.Id);
            var comment = new Comment { UserId = user.Id, PostId = post.Id, Description = "old", CreatedDate = DateTime.UtcNow };
            await _sut.AddAsync(comment, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var toUpdate = await _sut.GetByIdAsync(comment.Id, CancellationToken.None);
            toUpdate!.Description = "edited";
            await _sut.UpdateAsync(toUpdate, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var reloaded = await _sut.GetByIdAsync(comment.Id, CancellationToken.None);
            Assert.Equal("edited", reloaded!.Description);
        }

        [Fact]
        public async Task DeleteAsync_ThenSaveChanges_RemovesTheComment()
        {
            var user = await CreateUserAsync();
            var post = await CreatePostAsync(user.Id);
            var comment = new Comment { UserId = user.Id, PostId = post.Id, Description = "nice", CreatedDate = DateTime.UtcNow };
            await _sut.AddAsync(comment, CancellationToken.None);
            await Context.SaveChangesAsync();

            await _sut.DeleteAsync(comment, CancellationToken.None);
            await Context.SaveChangesAsync();

            var loaded = await _sut.GetByIdAsync(comment.Id, CancellationToken.None);
            Assert.Null(loaded);
        }

        [Fact]
        public async Task DeletingThePost_CascadesToItsComments()
        {
            var user = await CreateUserAsync();
            var post = await CreatePostAsync(user.Id);
            var comment = new Comment { UserId = user.Id, PostId = post.Id, Description = "nice", CreatedDate = DateTime.UtcNow };
            await _sut.AddAsync(comment, CancellationToken.None);
            await Context.SaveChangesAsync();

            Context.Posts.Remove(post);
            await Context.SaveChangesAsync();

            Assert.Empty(Context.Comments);
        }
    }
}
