using System.ComponentModel.DataAnnotations;

namespace Application.Dto
{
    public class UserRegisterRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}