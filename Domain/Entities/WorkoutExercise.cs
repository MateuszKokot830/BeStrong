using System.ComponentModel.DataAnnotations.Schema;
using Domain.Aggregates;
using Domain.Models;

namespace Domain.Entities
{
    [Table("WorkoutExercises")]
    public class WorkoutExercise : Entity<int>
    {
        public int Order { get; set; }
        public string? Notes { get; set; }
        public int ExerciseId { get; set; }
        [ForeignKey("ExerciseId")]
        public virtual Exercise? Exercise { get; set; }
        public int WorkoutId { get; set; }
        [ForeignKey("WorkoutId")]
        public virtual Workout? Workout { get; set; }
        public virtual ICollection<WorkoutSet> Sets { get; set; } = [];
    }
}
