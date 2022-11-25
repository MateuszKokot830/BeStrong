using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace Domain.Aggregates
{
    public class UserAggregate : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}