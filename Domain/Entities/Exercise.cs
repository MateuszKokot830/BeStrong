using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;

namespace Domain.Entities
{
    [Table("Exercises")]
    public class Exercise : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<WorkoutExercise> WorkoutExercise { get; set; }
    }
}