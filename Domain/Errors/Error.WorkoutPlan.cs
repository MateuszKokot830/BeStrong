using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class WorkoutPlan
    {
        public static Error NotFound => Error.NotFound(
            code: "WorkoutPlan.NotFound",
            description: "Workout plan was not found.");

        public static Error Unauthorized => Error.Unauthorized(
            code: "WorkoutPlan.Unauthorized",
            description: "You are not authorized to modify this workout plan.");

        public static Error DuplicateName => Error.Conflict(
            code: "WorkoutPlan.DuplicateName",
            description: "A workout plan with this name already exists.");
    }
}
