using System.ComponentModel.DataAnnotations;

namespace Application.Dto
{
    public class UserLoginRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}