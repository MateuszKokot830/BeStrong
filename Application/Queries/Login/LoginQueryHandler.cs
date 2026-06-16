using Application.Dto.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Login
{
    public class LoginQueryHandler(
        IUserRepository userRepository,
        ITokenService tokenService) : IRequestHandler<LoginQuery, ErrorOr<UserAuthResponseDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<ErrorOr<UserAuthResponseDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.UserLoginRequestDto.UserName, cancellationToken);
            if (user is null)
                return Errors.Auth.InvalidCredentials;

            var passwordValid = await _userRepository.CheckPasswordAsync(user, request.UserLoginRequestDto.Password, cancellationToken);
            if (!passwordValid)
                return Errors.Auth.InvalidCredentials;

            var token = await _tokenService.CreateTokenAsync(user.ToDto(), cancellationToken);

            return new UserAuthResponseDto(user.UserName, token);
        }
    }
}
