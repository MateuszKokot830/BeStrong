using ErrorOr;

namespace Domain.Errors;

public static partial class Errors
{
    public static class Photo
    {
        public static Error NotFound => Error.NotFound(
            code: "Photo.NotFound",
            description: "Photo was not found.");

        public static Error IsProfilePhoto => Error.Conflict(
            code: "Photo.IsProfilePhoto",
            description: "Cannot delete the profile photo.");

        // Returned as a real domain result when Cloudinary rejects or fails the upload.
        public static Error UploadFailed => Error.Failure(
            code: "Photo.UploadFailed",
            description: "Failed to upload the photo. Please try again.");
    }
}
