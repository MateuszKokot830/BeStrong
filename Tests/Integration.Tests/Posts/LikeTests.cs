using System.Net;
using System.Net.Http.Json;
using Application.Dto.Comment;
using Application.Dto.Post;
using Domain.Common;
using Integration.Tests.TestDoubles;

namespace Integration.Tests.Posts
{
    public class LikeTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public LikeTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private async Task<int> CreatePostAsync()
        {
            var response = await _client.PostAsJsonAsync("/api/posts", new PostCreateDto(PostType.Normal, "likeable post", null, null));
            var post = await response.Content.ReadFromJsonAsync<PostDto>();
            return post!.Id;
        }

        private async Task<int> CreateCommentAsync(int postId)
        {
            var response = await _client.PostAsJsonAsync("/api/posts/comments", new CommentCreateDto("likeable comment", postId));
            var comment = await response.Content.ReadFromJsonAsync<CommentDto>();
            return comment!.Id;
        }

        [Fact]
        public async Task LikePost_ThenUnlike_RoundTripsCorrectly()
        {
            var token = await _client.RegisterAndGetTokenAsync("liam_postlike");
            _client.SetBearerToken(token);
            var postId = await CreatePostAsync();

            var likeResponse = await _client.PostAsync($"/api/posts/{postId}/like", content: null);
            Assert.Equal(HttpStatusCode.NoContent, likeResponse.StatusCode);

            var unlikeResponse = await _client.DeleteAsync($"/api/posts/{postId}/like");
            Assert.Equal(HttpStatusCode.NoContent, unlikeResponse.StatusCode);
        }

        [Fact]
        public async Task LikePost_WhenAlreadyLiked_ReturnsConflict()
        {
            var token = await _client.RegisterAndGetTokenAsync("liam_dupelike");
            _client.SetBearerToken(token);
            var postId = await CreatePostAsync();
            await _client.PostAsync($"/api/posts/{postId}/like", content: null);

            var secondLikeResponse = await _client.PostAsync($"/api/posts/{postId}/like", content: null);

            Assert.Equal(HttpStatusCode.Conflict, secondLikeResponse.StatusCode);
        }

        [Fact]
        public async Task UnlikePost_WhenNotLiked_IsIdempotent()
        {
            var token = await _client.RegisterAndGetTokenAsync("liam_neverliked");
            _client.SetBearerToken(token);
            var postId = await CreatePostAsync();

            var response = await _client.DeleteAsync($"/api/posts/{postId}/like");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task LikePost_WhenPostDoesNotExist_ReturnsNotFound()
        {
            var token = await _client.RegisterAndGetTokenAsync("liam_ghostlike");
            _client.SetBearerToken(token);

            var response = await _client.PostAsync("/api/posts/999999/like", content: null);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task LikeComment_ThenUnlike_RoundTripsCorrectly()
        {
            var token = await _client.RegisterAndGetTokenAsync("liam_commentlike");
            _client.SetBearerToken(token);
            var postId = await CreatePostAsync();
            var commentId = await CreateCommentAsync(postId);

            var likeResponse = await _client.PostAsync($"/api/posts/comments/{commentId}/like", content: null);
            Assert.Equal(HttpStatusCode.NoContent, likeResponse.StatusCode);

            var unlikeResponse = await _client.DeleteAsync($"/api/posts/comments/{commentId}/like");
            Assert.Equal(HttpStatusCode.NoContent, unlikeResponse.StatusCode);
        }

        [Fact]
        public async Task LikeComment_WhenAlreadyLiked_ReturnsConflict()
        {
            var token = await _client.RegisterAndGetTokenAsync("liam_dupecommentlike");
            _client.SetBearerToken(token);
            var postId = await CreatePostAsync();
            var commentId = await CreateCommentAsync(postId);
            await _client.PostAsync($"/api/posts/comments/{commentId}/like", content: null);

            var secondLikeResponse = await _client.PostAsync($"/api/posts/comments/{commentId}/like", content: null);

            Assert.Equal(HttpStatusCode.Conflict, secondLikeResponse.StatusCode);
        }
    }
}
