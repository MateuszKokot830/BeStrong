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
        }
    }
}