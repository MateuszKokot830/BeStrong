using Domain.Common;
using Domain.Models;

namespace Domain.ValueObjects
{
    public class UserSettings : ValueObject
    {
        public ProfileVisibility PhotosVisibility { get; set; }
        public ProfileVisibility WorkoutsVisibility { get; set; }
        public ProfileVisibility WorkoutPlanVisibility { get; set; }
        public ProfileVisibility MeasurementsVisibility { get; set; }
        public bool AutoPublishWorkouts { get; set; }
        public bool AutoPublishWorkoutPlanChanges { get; set; }

        public UserSettings(
            ProfileVisibility photosVisibility,
            ProfileVisibility workoutsVisibility,
            ProfileVisibility workoutPlanVisibility,
            ProfileVisibility measurementsVisibility,
            bool autoPublishWorkouts,
            bool autoPublishWorkoutPlanChanges)
        {
            PhotosVisibility = photosVisibility;
            WorkoutsVisibility = workoutsVisibility;
            WorkoutPlanVisibility = workoutPlanVisibility;
            MeasurementsVisibility = measurementsVisibility;
            AutoPublishWorkouts = autoPublishWorkouts;
            AutoPublishWorkoutPlanChanges = autoPublishWorkoutPlanChanges;
        }

        public UserSettings() { }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PhotosVisibility;
            yield return WorkoutsVisibility;
            yield return WorkoutPlanVisibility;
            yield return MeasurementsVisibility;
            yield return AutoPublishWorkouts;
            yield return AutoPublishWorkoutPlanChanges;
        }
    }
}
