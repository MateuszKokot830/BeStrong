using Application.Commands.Users.AddPhoto;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class AddPhotoCommandValidator : AbstractValidator<AddPhotoCommand>
    {
        private static readonly HashSet<string> AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

        public AddPhotoCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");

            RuleFor(x => x.File)
                .NotNull().WithMessage("Photo file is required.");

            When(x => x.File is not null, () =>
            {
                RuleFor(x => x.File.Length)
                    .GreaterThan(0).WithMessage("Photo file cannot be empty.")
                    .LessThanOrEqualTo(MaxFileSizeBytes).WithMessage("Photo file cannot exceed 10 MB.");

                RuleFor(x => Path.GetExtension(x.File.FileName).ToLowerInvariant())
                    .Must(ext => AllowedExtensions.Contains(ext))
                    .WithMessage("Only .jpg, .jpeg, .png, .gif and .webp files are allowed.");
            });
        }
    }
}
