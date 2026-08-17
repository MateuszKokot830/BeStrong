using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class WorkoutPlan
    {
        public static Error NotFound => Error.NotFound(
            code: "WorkoutPlan.NotFound",
            description: "Workout plan was not found.");

        public static Error Forbidden => Error.Forbidden(
            code: "WorkoutPlan.Forbidden",
            description: "You are not authorized to modify this workout plan.");

        public static Error DuplicateName => Error.Conflict(
            code: "WorkoutPlan.DuplicateName",
            description: "A workout plan with this name already exists.");

        public static Error InUse => Error.Conflict(
            code: "WorkoutPlan.InUse",
            description: "This workout plan is currently in use and cannot be modified.");
    }
}
