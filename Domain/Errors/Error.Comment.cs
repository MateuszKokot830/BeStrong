using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class Comment
    {
        public static Error NotFound => Error.NotFound(
            code: "Comment.NotFound",
            description: "Comment was not found.");

        public static Error Unauthorized => Error.Unauthorized(
            code: "Comment.Unauthorized",
            description: "You are not authorized to delete this comment.");

        public static Error AlreadyLiked => Error.Conflict(
            code: "Comment.AlreadyLiked",
            description: "You have already liked this comment.");
    }
}
