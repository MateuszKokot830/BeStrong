using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Domain.Models;

namespace Domain.Aggregates
{
    [Table("Posts")]
    public class Post : AggregateRoot<int>
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate => DateTime.Now;
        public int? WorkoutId { get; set; }
        public virtual Workout Workout { get; set; }
        public int Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}