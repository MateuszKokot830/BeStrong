using System.ComponentModel.DataAnnotations;

namespace Application.Dto
{
    public class UserLoginRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}