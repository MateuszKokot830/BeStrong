using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Exercises")]
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual WorkoutExercise WorkoutExercise { get; set; }
    }
}