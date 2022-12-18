using System.ComponentModel.DataAnnotations;

namespace Application.Dto
{
    public class UserRegisterRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}