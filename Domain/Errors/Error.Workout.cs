using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class Workout
    {
        public static Error NotFound => Error.NotFound(
            code: "Workout.NotFound",
            description: "Workout was not found");

        public static Error Unauthorized => Error.Unauthorized(
            code: "Workout.Unauthorized",
            description: "You are not authorized to modify this workout");

        public static Error InvalidInput => Error.Validation(
            code: "Workout.InvalidInput",
            description: "Invalid workout data provided");

        public static Error CreationFailed => Error.Failure(
            code: "Workout.CreationFailed",
            description: "Failed to create workout");

        public static Error UpdateFailed => Error.Failure(
            code: "Workout.UpdateFailed",
            description: "Failed to update workout");

        public static Error DeletionFailed => Error.Failure(
            code: "Workout.DeletionFailed",
            description: "Failed to delete workout");
    }
}