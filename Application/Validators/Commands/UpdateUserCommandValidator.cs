using Application.Commands.Users.UpdateUser;
using Application.Validators.Common;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserUpdateDto.Id)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");

            RuleFor(x => x.UserUpdateDto.DateOfBirth)
                .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.")
                .GreaterThan(DateTime.UtcNow.AddYears(-120)).WithMessage("Date of birth is not realistic.");

            RuleFor(x => x.UserUpdateDto.DateOfWorkoutStart!.Value)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Workout start date cannot be in the future.")
                .GreaterThan(DateTime.UtcNow.AddYears(-100)).WithMessage("Workout start date is not realistic.")
                .When(x => x.UserUpdateDto.DateOfWorkoutStart.HasValue);

            RuleFor(x => x.UserUpdateDto.Name)
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.")
                .When(x => x.UserUpdateDto.Name is not null);

            RuleFor(x => x.UserUpdateDto.Surname)
                .MaximumLength(50).WithMessage("Surname cannot exceed 50 characters.")
                .When(x => x.UserUpdateDto.Surname is not null);

            RuleFor(x => x.UserUpdateDto.City)
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters.")
                .When(x => x.UserUpdateDto.City is not null);

            RuleFor(x => x.UserUpdateDto.Country)
                .MaximumLength(100).WithMessage("Country cannot exceed 100 characters.")
                .When(x => x.UserUpdateDto.Country is not null);

            RuleFor(x => x.UserUpdateDto.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
                .When(x => x.UserUpdateDto.Description is not null);

            RuleFor(x => x.UserUpdateDto.Measurements!)
                .SetValidator(new MeasurementsDtoValidator())
                .When(x => x.UserUpdateDto.Measurements is not null);
        }
    }
}
