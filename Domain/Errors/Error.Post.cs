using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class Post
    {
        public static Error NotFound => Error.NotFound(
            code: "Post.NotFound",
            description: "Post was not found");

        public static Error Unauthorized => Error.Unauthorized(
            code: "Post.Unauthorized",
            description: "You are not authorized to delete this post");

        public static Error InvalidInput => Error.Validation(
            code: "Post.InvalidInput",
            description: "Invalid post data provided");

        public static Error CreationFailed => Error.Failure(
            code: "Post.CreationFailed",
            description: "Failed to create post");

        public static Error DeletionFailed => Error.Failure(
            code: "Post.DeletionFailed",
            description: "Failed to delete post");
    }
}