using Application.Interfaces;

namespace Infrastructure.Services
{
    public class CalculatorService : ICalculatorService
    {
        public int CalculateOneRepMax(int weight, int reps)
        {
            if (reps == 0) return 0;

            return (int)Math.Ceiling(weight / ( 1.0278 - 0.0278 * reps ));
        }
    }
}