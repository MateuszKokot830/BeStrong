namespace Application.Dto.Auth
{
    public record UserRegisterRequestDto(
        string UserName,
        string Password
    );
}