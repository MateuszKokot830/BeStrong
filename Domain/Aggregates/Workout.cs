using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Domain.Models;

namespace Domain.Aggregates
{
    [Table("Workouts")]
    public class Workout : AggregateRoot<int>
    {
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public IEnumerable<WorkoutExercise> WorkoutExercises { get; set; }
    }
}