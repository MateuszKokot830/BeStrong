using Domain.ValueObjects;
using MediatR;

namespace Application.Commands.Users.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfWorkoutStart { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public Measurements Measurments { get; set; }
    }
}