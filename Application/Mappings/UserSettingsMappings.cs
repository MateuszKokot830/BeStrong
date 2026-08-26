using Application.Dto.User;
using Domain.Common;
using Domain.ValueObjects;

namespace Application.Mappings
{
    public static class UserSettingsMappings
    {
        public static readonly UserSettingsDto Default = new(
            ProfileVisibility.Public,
            ProfileVisibility.Public,
            ProfileVisibility.Public,
            ProfileVisibility.Public,
            AutoPublishWorkouts: true,
            AutoPublishWorkoutPlanChanges: true);

        public static UserSettingsDto ToDto(this UserSettings settings) => new(
            settings.PhotosVisibility,
            settings.WorkoutsVisibility,
            settings.WorkoutPlanVisibility,
            settings.MeasurementsVisibility,
            settings.AutoPublishWorkouts,
            settings.AutoPublishWorkoutPlanChanges
        );

        public static UserSettingsDto ToDtoOrDefault(this UserSettings? settings) =>
            settings?.ToDto() ?? Default;

        public static UserSettings ToEntity(this UserSettingsDto dto) => new(
            dto.PhotosVisibility,
            dto.WorkoutsVisibility,
            dto.WorkoutPlanVisibility,
            dto.MeasurementsVisibility,
            dto.AutoPublishWorkouts,
            dto.AutoPublishWorkoutPlanChanges
        );
    }
}
