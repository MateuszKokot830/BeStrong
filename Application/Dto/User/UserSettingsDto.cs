using Domain.Common;

namespace Application.Dto.User
{
    public record UserSettingsDto(
        ProfileVisibility PhotosVisibility,
        ProfileVisibility WorkoutsVisibility,
        ProfileVisibility WorkoutPlanVisibility,
        ProfileVisibility MeasurementsVisibility,
        bool AutoPublishWorkouts,
        bool AutoPublishWorkoutPlanChanges
    );
}
