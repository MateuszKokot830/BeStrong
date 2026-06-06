namespace Application.Dto.Auth
{
    public class UserRegisterRequestDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }

    }
}