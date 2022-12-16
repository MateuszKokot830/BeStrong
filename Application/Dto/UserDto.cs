namespace Application.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } 
        public DateTime CreatedDate { get; set;} = DateTime.UtcNow;
        public DateTime DateOfBirth { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public virtual ICollection<PhotoDto> Photos { get; set; }
    }
}