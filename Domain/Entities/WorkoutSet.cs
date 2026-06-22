using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;

namespace Domain.Entities
{
    [Table("WorkoutSets")]
    public class WorkoutSet : Entity<int>
    {
        public int SetNumber { get; set; }
        public int Reps { get; set; }
        public decimal? Weight { get; set; }
        public int WorkoutExerciseId { get; set; }
        [ForeignKey("WorkoutExerciseId")]
        public virtual WorkoutExercise? WorkoutExercise { get; set; }
    }
}
