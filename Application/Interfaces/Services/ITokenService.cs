using Application.Dto.Auth;

namespace Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string?> CreateTokenAsync(CreateTokenRequest request, CancellationToken cancellationToken = default);
    }
}
