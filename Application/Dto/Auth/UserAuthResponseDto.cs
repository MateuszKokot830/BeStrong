namespace Application.Dto.Auth
{
    public record UserAuthResponseDto(
        string? Username,
        string? Token
    );
}