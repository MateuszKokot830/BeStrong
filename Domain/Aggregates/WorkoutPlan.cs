using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;

namespace Domain.Aggregates
{
    [Table("WorkoutPlans")]
    public class WorkoutPlan : AggregateRoot<int>
    {
        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual ICollection<User> UsedBy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Workout> Workouts { get; set; }
    }
}