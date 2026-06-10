using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class Post
    {
        public static Error NotFound => Error.NotFound(
            code: "Post.NotFound",
            description: "Post was not found.");

        public static Error Unauthorized => Error.Unauthorized(
            code: "Post.Unauthorized",
            description: "You are not authorized to delete this post.");
    }
}
