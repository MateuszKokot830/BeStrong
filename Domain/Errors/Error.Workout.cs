using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class Workout
    {
        public static Error NotFound => Error.NotFound(
            code: "Workout.NotFound",
            description: "Workout was not found.");

        public static Error Unauthorized => Error.Unauthorized(
            code: "Workout.Unauthorized",
            description: "You are not authorized to modify this workout.");
    }
}
