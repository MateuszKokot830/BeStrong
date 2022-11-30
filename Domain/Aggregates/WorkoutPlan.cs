using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Domain.Aggregates
{
    [Table("WorkoutPlans")]
    public class WorkoutPlan
    {
        public int Id { get; set; }
        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual IEnumerable<User> UsedBy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Workout> Workouts { get; set; }
    }
}