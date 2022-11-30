using System.ComponentModel.DataAnnotations.Schema;
using Domain.Aggregates;

namespace Domain.Entities
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Likes { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}