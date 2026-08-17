using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class Workout
    {
        public static Error NotFound => Error.NotFound(
            code: "Workout.NotFound",
            description: "Workout was not found.");

        public static Error Forbidden => Error.Forbidden(
            code: "Workout.Forbidden",
            description: "You are not authorized to modify this workout.");
    }
}
