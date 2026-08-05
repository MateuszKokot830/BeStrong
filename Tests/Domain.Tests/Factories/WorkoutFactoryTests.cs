using Domain.Entities;
using Domain.Factories;

namespace Domain.Tests.Factories
{
    public class WorkoutFactoryTests
    {
        [Fact]
        public void Create_MapsUserIdNameAndExercises()
        {
            var exercises = new List<WorkoutExercise> { new() { Order = 1, ExerciseId = 5 } };

            var workout = WorkoutFactory.Create(userId: 42, name: "Push Day", exercises);

            Assert.Equal(42, workout.UserId);
            Assert.Equal("Push Day", workout.Name);
            Assert.Single(workout.WorkoutExercises);
            Assert.Same(exercises[0], workout.WorkoutExercises.First());
        }

        [Fact]
        public void Create_SetsDateToUtcNow()
        {
            var before = DateTime.UtcNow;

            var workout = WorkoutFactory.Create(userId: 1, name: null, exercises: []);

            var after = DateTime.UtcNow;
            Assert.InRange(workout.Date, before, after);
        }

        [Fact]
        public void Create_WithNullName_IsAllowed()
        {
            var workout = WorkoutFactory.Create(userId: 1, name: null, exercises: []);

            Assert.Null(workout.Name);
        }
    }
}
