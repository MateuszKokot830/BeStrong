using ErrorOr;

namespace Domain.Errors
{
    public static partial class Errors
    {
        public static class Auth
        {
            public static Error InvalidCredentials => Error.Unauthorized(
                code: "Auth.InvalidCredentials",
                description: "The provided credentials are incorrect.");
        }
    }
}
