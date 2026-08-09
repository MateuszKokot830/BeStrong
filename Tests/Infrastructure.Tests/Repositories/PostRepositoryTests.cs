using Domain.Aggregates;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Tests.TestDoubles;

namespace Infrastructure.Tests.Repositories
{
    public class PostRepositoryTests : SqliteInMemoryFixture
    {
        private readonly PostRepository _sut;

        public PostRepositoryTests()
        {
            _sut = new PostRepository(Context);
        }

        [Fact]
        public async Task AddAsync_ThenSaveChanges_PersistsThePost()
        {
            var user = await CreateUserAsync();
            var post = new Post { UserId = user.Id, Type = PostType.Normal, Description = "hi", CreatedDate = DateTime.UtcNow };

            await _sut.AddAsync(post, CancellationToken.None);
            await Context.SaveChangesAsync();

            var loaded = await _sut.GetByIdAsync(post.Id, CancellationToken.None);
            Assert.NotNull(loaded);
            Assert.Equal("hi", loaded!.Description);
        }

        [Fact]
        public async Task GetByIdAsync_IncludesLikes()
        {
            var user = await CreateUserAsync();
            var post = new Post { UserId = user.Id, Type = PostType.Normal, Description = "hi", CreatedDate = DateTime.UtcNow };
            await _sut.AddAsync(post, CancellationToken.None);
            await Context.SaveChangesAsync();

            await _sut.AddLikeAsync(new PostLike { UserId = user.Id, PostId = post.Id }, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var loaded = await _sut.GetByIdAsync(post.Id, CancellationToken.None);

            Assert.Single(loaded!.Likes);
        }

        [Fact]
        public async Task DeleteLikeAsync_ThenSaveChanges_RemovesTheLike()
        {
            var user = await CreateUserAsync();
            var post = new Post { UserId = user.Id, Type = PostType.Normal, Description = "hi", CreatedDate = DateTime.UtcNow };
            await _sut.AddAsync(post, CancellationToken.None);
            await Context.SaveChangesAsync();
            var like = new PostLike { UserId = user.Id, PostId = post.Id };
            await _sut.AddLikeAsync(like, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var reloadedPost = await _sut.GetByIdAsync(post.Id, CancellationToken.None);
            var reloadedLike = reloadedPost!.Likes.Single();
            await _sut.DeleteLikeAsync(reloadedLike, CancellationToken.None);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var finalPost = await _sut.GetByIdAsync(post.Id, CancellationToken.None);
            Assert.Empty(finalPost!.Likes);
        }

        [Fact]
        public async Task DeleteAsync_CascadesToLikes()
        {
            var user = await CreateUserAsync();
            var post = new Post { UserId = user.Id, Type = PostType.Normal, Description = "hi", CreatedDate = DateTime.UtcNow };
            await _sut.AddAsync(post, CancellationToken.None);
            await Context.SaveChangesAsync();
            await _sut.AddLikeAsync(new PostLike { UserId = user.Id, PostId = post.Id }, CancellationToken.None);
            await Context.SaveChangesAsync();

            await _sut.DeleteAsync(post, CancellationToken.None);
            await Context.SaveChangesAsync();

            Assert.Empty(Context.PostLikes);
        }
    }
}
