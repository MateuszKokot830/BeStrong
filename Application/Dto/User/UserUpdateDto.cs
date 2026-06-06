using Application.Dto.Photo;
using Domain.Common;

namespace Application.Dto.User
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfWorkoutStart { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public Gender Gender { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public MeasurementsDto? Measurements { get; set; }
        public ICollection<PhotoDto> Photos { get; set; } = [];
    }
}