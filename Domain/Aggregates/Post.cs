using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entities;
using Domain.Models;

namespace Domain.Aggregates
{
    [Table("Posts")]
    public class Post : AggregateRoot<int>
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        public PostType Type { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? WorkoutId { get; set; }
        [ForeignKey("WorkoutId")]
        public virtual Workout? Workout { get; set; }
        public int? WorkoutPlanId { get; set; }
        [ForeignKey("WorkoutPlanId")]
        public virtual WorkoutPlan? WorkoutPlan { get; set; }
        public virtual ICollection<PostLike> Likes { get; set; } = [];
        public virtual ICollection<Comment> Comments { get; set; } = [];
    }
}
