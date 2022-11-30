using System.ComponentModel.DataAnnotations.Schema;
using Domain.Aggregates;

namespace Domain.Entities
{
    [Table("WorkoutExercises")]
    public class WorkoutExercise
    {
        public int Id { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public Decimal? Weight { get; set; }
        public int ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }
        public int WorkoutId { get; set; }
        public virtual Workout Workout { get; set; }
    }
}