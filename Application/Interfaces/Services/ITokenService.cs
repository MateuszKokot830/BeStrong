using Application.Dto.User;

namespace Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string?> CreateTokenAsync(UserDto user, CancellationToken cancellationToken = default);
    }
}