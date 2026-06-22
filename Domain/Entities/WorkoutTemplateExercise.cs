using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;

namespace Domain.Entities
{
    [Table("WorkoutTemplateExercises")]
    public class WorkoutTemplateExercise : Entity<int>
    {
        public int Order { get; set; }
        public int ExerciseId { get; set; }
        [ForeignKey("ExerciseId")]
        public virtual Exercise? Exercise { get; set; }
        public int WorkoutTemplateId { get; set; }
        [ForeignKey("WorkoutTemplateId")]
        public virtual WorkoutTemplate? WorkoutTemplate { get; set; }
    }
}
