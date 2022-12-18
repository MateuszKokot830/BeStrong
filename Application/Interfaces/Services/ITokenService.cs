using Application.Dto;

namespace Application.Interfaces
{
    public interface ITokenService
    {
         string CreateToken(UserDto user);
    }
}