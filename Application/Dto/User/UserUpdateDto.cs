namespace Application.Dto
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfWorkoutStart { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public MeasurementsDto Measurements { get; set; }
    }
}