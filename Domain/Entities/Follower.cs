using System.ComponentModel.DataAnnotations.Schema;
using Domain.Aggregates;

namespace Domain.Entities
{
    [Table("Followers")]
    public class Follower
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int FollowedUserId { get; set; }
        public virtual User FollowedUser { get; set; }
    }
}