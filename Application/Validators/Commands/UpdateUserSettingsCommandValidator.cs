using Application.Commands.Users.UpdateUserSettings;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class UpdateUserSettingsCommandValidator : AbstractValidator<UpdateUserSettingsCommand>
    {
        public UpdateUserSettingsCommandValidator()
        {
            RuleFor(x => x.SettingsDto.PhotosVisibility).IsInEnum();
            RuleFor(x => x.SettingsDto.WorkoutsVisibility).IsInEnum();
            RuleFor(x => x.SettingsDto.WorkoutPlanVisibility).IsInEnum();
            RuleFor(x => x.SettingsDto.MeasurementsVisibility).IsInEnum();
        }
    }
}
