using Application.Dto.User;
using FluentValidation;

namespace Application.Validators.Common
{
    public sealed class MeasurementsDtoValidator : AbstractValidator<MeasurementsDto>
    {
        public MeasurementsDtoValidator()
        {
            RuleFor(x => x.Height)
                .InclusiveBetween(50, 300).WithMessage("Height must be between 50 and 300 cm.")
                .When(x => x.Height.HasValue);

            RuleFor(x => x.Weight)
                .InclusiveBetween(20, 700).WithMessage("Weight must be between 20 and 700 kg.")
                .When(x => x.Weight.HasValue);

            RuleFor(x => x.Chest)
                .GreaterThan(0).WithMessage("Chest measurement must be positive.")
                .LessThanOrEqualTo(300).WithMessage("Chest measurement seems unrealistic.")
                .When(x => x.Chest.HasValue);

            RuleFor(x => x.Shoulders)
                .GreaterThan(0).WithMessage("Shoulders measurement must be positive.")
                .LessThanOrEqualTo(300).WithMessage("Shoulders measurement seems unrealistic.")
                .When(x => x.Shoulders.HasValue);

            RuleFor(x => x.Arms)
                .GreaterThan(0).WithMessage("Arms measurement must be positive.")
                .LessThanOrEqualTo(200).WithMessage("Arms measurement seems unrealistic.")
                .When(x => x.Arms.HasValue);

            RuleFor(x => x.Waist)
                .GreaterThan(0).WithMessage("Waist measurement must be positive.")
                .LessThanOrEqualTo(300).WithMessage("Waist measurement seems unrealistic.")
                .When(x => x.Waist.HasValue);

            RuleFor(x => x.Hips)
                .GreaterThan(0).WithMessage("Hips measurement must be positive.")
                .LessThanOrEqualTo(300).WithMessage("Hips measurement seems unrealistic.")
                .When(x => x.Hips.HasValue);

            RuleFor(x => x.Thights)
                .GreaterThan(0).WithMessage("Thighs measurement must be positive.")
                .LessThanOrEqualTo(200).WithMessage("Thighs measurement seems unrealistic.")
                .When(x => x.Thights.HasValue);
        }
    }
}
