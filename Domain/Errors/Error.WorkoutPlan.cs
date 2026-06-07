using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class WorkoutPlan
    {
        public static Error NotFound => Error.NotFound(
            code: "WorkoutPlan.NotFound",
            description: "Workout plan was not found");

        public static Error Unauthorized => Error.Unauthorized(
            code: "WorkoutPlan.Unauthorized",
            description: "You are not authorized to modify this workout plan");

        public static Error InvalidInput => Error.Validation(
            code: "WorkoutPlan.InvalidInput",
            description: "Invalid workout plan data provided");

        public static Error CreationFailed => Error.Failure(
            code: "WorkoutPlan.CreationFailed",
            description: "Failed to create workout plan");

        public static Error UpdateFailed => Error.Failure(
            code: "WorkoutPlan.UpdateFailed",
            description: "Failed to update workout plan");

        public static Error DeletionFailed => Error.Failure(
            code: "WorkoutPlan.DeletionFailed",
            description: "Failed to delete workout plan");

        public static Error DuplicateName => Error.Validation(
            code: "WorkoutPlan.DuplicateName",
            description: "Workout plan with this name already exists");
    }
}