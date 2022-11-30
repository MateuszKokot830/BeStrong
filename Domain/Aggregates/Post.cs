using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Domain.Aggregates
{
    [Table("Posts")]
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int? WorkoutId { get; set; }
        public virtual Workout Workout { get; set; }
        public int Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}