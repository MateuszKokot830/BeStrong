using Application.Dto.Photo;
using Domain.Common;

namespace Application.Dto.User
{
    public record UserUpdateDto(
        int Id,
        DateTime DateOfBirth,
        DateTime? DateOfWorkoutStart,
        string? Name,
        string? Surname,
        Gender Gender,
        string? City,
        string? Country,
        string? Description,
        MeasurementsDto? Measurements,
        IReadOnlyCollection<PhotoDto> Photos
    );
}
