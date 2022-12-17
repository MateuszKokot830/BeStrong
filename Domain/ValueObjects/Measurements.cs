using Domain.Models;

namespace Domain.ValueObjects
{
    public class Measurements : ValueObject
    {
        public int? Height { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Chest { get; set; }
        public decimal? Shoulders { get; set; }
        public decimal? Arms { get; set; }
        public decimal? Waist { get; set; }
        public decimal? Hips { get; set; }
        public decimal? Thights { get; set; }

        public Measurements(int? height, decimal? weight, decimal? chest, decimal? shoulders, decimal? arms, decimal? waist, decimal? hips, decimal? thights)
        {
            Height = height;
            Weight = weight;
            Chest = chest;
            Shoulders = shoulders;
            Arms = arms;
            Waist = waist;
            Hips = hips;
            Thights = thights;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Height;
            yield return Weight;
            yield return Chest;
            yield return Shoulders;
            yield return Arms;
            yield return Waist;
            yield return Hips;
            yield return Thights;
        }
    }
}