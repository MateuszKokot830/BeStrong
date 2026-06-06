namespace Application.Dto.Auth
{
    public class UserLoginRequestDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}