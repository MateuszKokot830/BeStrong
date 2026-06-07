using ErrorOr;

namespace Domain.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            public static Error DuplicateUsername => Error.Validation(
                code: "User.DuplicateUsername",
                description: "User with given username already exists");
            public static Error FailedRegister => Error.Validation(
                code: "User.FailedRegister",
                description: "Please provide a stronger password");
            public static Error NotFound => Error.NotFound(
                code: "User.NotFound",
                description: "User was not found");

            public static Error Unauthorized => Error.Unauthorized(
                code: "User.Unauthorized",
                description: "User is not authorized to perform this action");

            public static Error InvalidInput => Error.Validation(
                code: "User.InvalidInput",
                description: "Invalid user input provided");

            public static Error UpdateFailed => Error.Failure(
                code: "User.UpdateFailed",
                description: "Failed to update user");
        }
    }
}