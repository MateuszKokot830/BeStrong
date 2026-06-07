using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class Photo
    {
        public static Error NotFound => Error.NotFound(
            code: "Photo.NotFound",
            description: "Photo was not found");

        public static Error IsProfilePhoto => Error.Validation(
            code: "Photo.IsProfilePhoto",
            description: "Cannot delete the profile photo");

        public static Error UploadFailed => Error.Failure(
            code: "Photo.UploadFailed",
            description: "Failed to upload photo");

        public static Error DeletionFailed => Error.Failure(
            code: "Photo.DeletionFailed",
            description: "Failed to delete photo");
    }
}
