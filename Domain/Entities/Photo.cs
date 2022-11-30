using System.ComponentModel.DataAnnotations.Schema;
using Domain.Aggregates;

namespace Domain.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; } 
        public bool IsProfilePhoto { get; set; }
        public int UserId { get; set; }
        public virtual User User{ get; set; }
    }
}