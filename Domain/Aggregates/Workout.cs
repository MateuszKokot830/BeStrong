using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Domain.Aggregates
{
    [Table("Workouts")]
    public class Workout
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public IEnumerable<WorkoutExercise> WorkoutExercises { get; set; }
    }
}