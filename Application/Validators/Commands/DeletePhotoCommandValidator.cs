using Application.Commands.Users.DeletePhoto;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class DeletePhotoCommandValidator : AbstractValidator<DeletePhotoCommand>
    {
        public DeletePhotoCommandValidator()
        {
            RuleFor(x => x.PhotoId)
                .GreaterThan(0).WithMessage("PhotoId must be a valid positive integer.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");
        }
    }
}
