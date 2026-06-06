using ErrorOr;

namespace Domain.Errors
{
    public static partial class Errors
    {
        public static class Auth
        {
            public static Error InvalidUsername => Error.Failure(
                code: "Auth.InvalidUsername",
                description: "Invalid username");

            public static Error InvalidPassword => Error.Failure(
                code: "Auth.InvalidPassword",
                description: "Invalid password");
        }
    }
}