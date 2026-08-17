using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class Post
    {
        public static Error NotFound => Error.NotFound(
            code: "Post.NotFound",
            description: "Post was not found.");

        public static Error Forbidden => Error.Forbidden(
            code: "Post.Forbidden",
            description: "You are not authorized to delete this post.");

        public static Error AlreadyLiked => Error.Conflict(
            code: "Post.AlreadyLiked",
            description: "You have already liked this post.");

        public static Error WorkoutRequiredForPublication => Error.Validation(
            code: "Post.WorkoutRequiredForPublication",
            description: "A workout publication must reference a workout.");

        public static Error DescriptionRequired => Error.Validation(
            code: "Post.DescriptionRequired",
            description: "A normal post must have a description.");
    }
}
