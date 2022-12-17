using System.ComponentModel.DataAnnotations.Schema;
using Domain.Aggregates;
using Domain.Models;

namespace Domain.Entities
{
    [Table("Photos")]
    public class Photo : Entity<int>
    {
        public string PublicId { get; set; }
        public string Url { get; set; } 
        public bool IsProfilePhoto { get; set; }
        public int UserId { get; set; }
        public virtual User User{ get; set; }
    }
}