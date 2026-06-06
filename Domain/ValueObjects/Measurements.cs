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

        public Measurements() { }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Height ?? 0;
            yield return Weight ?? 0m;
            yield return Chest ?? 0m;
            yield return Shoulders ?? 0m;
            yield return Arms ?? 0m;
            yield return Waist ?? 0m;
            yield return Hips ?? 0m;
            yield return Thights ?? 0m;
        }
    }
}