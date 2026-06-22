using System.ComponentModel.DataAnnotations.Schema;
using Domain.Aggregates;
using Domain.Models;

namespace Domain.Entities
{
    [Table("WorkoutTemplates")]
    public class WorkoutTemplate : Entity<int>
    {
        public int Order { get; set; }
        public string? Name { get; set; }
        public int WorkoutPlanId { get; set; }
        [ForeignKey("WorkoutPlanId")]
        public virtual WorkoutPlan? WorkoutPlan { get; set; }
        public virtual ICollection<WorkoutTemplateExercise> Exercises { get; set; } = [];
    }
}
