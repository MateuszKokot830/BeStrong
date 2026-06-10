using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class Exercise
    {
        public static Error NotFound => Error.NotFound(
            code: "Exercise.NotFound",
            description: "Exercise was not found.");
    }
}
