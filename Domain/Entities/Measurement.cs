using System.ComponentModel.DataAnnotations.Schema;
using Domain.Aggregates;

namespace Domain.Entities
{
    [Table("Measurements")]
    public class Measurement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime Date { get; set; }
        public decimal Weight { get; set; }
        public decimal? Chest { get; set; }
        public decimal? Shoulders { get; set; }
        public decimal? Arms { get; set; }
        public decimal? Waist { get; set; }
        public decimal? Hips { get; set; }
        public decimal? Thights { get; set; }

    }
}