using System.ComponentModel.DataAnnotations.Schema;
using Domain.Aggregates;
using Domain.Models;

namespace Domain.Entities
{
    [Table("Followers")]
    public class Follower : Entity<int>
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        public int FollowedUserId { get; set; }
        [ForeignKey("FollowedUserId")]
        public virtual User? FollowedUser { get; set; }
        public DateTime FollowedAt { get; set; }
    }
}