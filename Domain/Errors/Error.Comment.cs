using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class Comment
    {
        public static Error NotFound => Error.NotFound(
            code: "Comment.NotFound",
            description: "Comment was not found");

        public static Error Unauthorized => Error.Unauthorized(
            code: "Comment.Unauthorized",
            description: "You are not authorized to delete this comment");

        public static Error InvalidInput => Error.Validation(
            code: "Comment.InvalidInput",
            description: "Invalid comment data provided");

        public static Error CreationFailed => Error.Failure(
            code: "Comment.CreationFailed",
            description: "Failed to create comment");

        public static Error DeletionFailed => Error.Failure(
            code: "Comment.DeletionFailed",
            description: "Failed to delete comment");
    }
}