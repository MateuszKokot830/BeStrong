using Application.Commands.Users.SetMainPhoto;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class SetMainPhotoCommandValidator : AbstractValidator<SetMainPhotoCommand>
    {
        public SetMainPhotoCommandValidator()
        {
            RuleFor(x => x.PhotoId)
                .GreaterThan(0).WithMessage("PhotoId must be a valid positive integer.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");
        }
    }
}
