using Application.Dto;

namespace Application.Interfaces
{
    public interface ICalculatorService
    {
         int CalculateOneRepMax(int weight, int reps);
    }
}