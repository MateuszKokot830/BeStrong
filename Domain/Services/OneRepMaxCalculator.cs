namespace Domain.Services
{
    public static class OneRepMaxCalculator
    {
        public static int Calculate(decimal weight, int reps) =>
            (int)Math.Ceiling((double)weight / (1.0278 - 0.0278 * reps));
    }
}
