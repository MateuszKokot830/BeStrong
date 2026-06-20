using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Models;

namespace Domain.Entities
{
    [Table("Exercises")]
    public class Exercise : Entity<int>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public MuscleSubgroup MuscleSubgroup { get; set; }
        public string? ImageUrl { get; set; }
        public virtual ICollection<WorkoutExercise> WorkoutExercise { get; set; } = [];
    }
}