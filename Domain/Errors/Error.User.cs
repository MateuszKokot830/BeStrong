using ErrorOr;

namespace Domain.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            public static Error DuplicateUsername => Error.Conflict(
                code: "User.DuplicateUsername",
                description: "A user with the given username already exists.");

            public static Error NotFound => Error.NotFound(
                code: "User.NotFound",
                description: "User was not found.");

            public static Error Unauthorized => Error.Unauthorized(
                code: "User.Unauthorized",
                description: "You are not authorized to perform this action.");
        }
    }
}
