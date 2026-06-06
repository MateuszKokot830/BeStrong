namespace Application.Dto.Auth
{
    public record UserLoginRequestDto(
        string UserName,
        string Password
    );
}