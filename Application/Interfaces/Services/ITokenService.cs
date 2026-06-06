using Application.Dto.User;

namespace Application.Interfaces.Services
{
    public interface ITokenService
    {
        string? CreateToken(UserDto user);
    }
}