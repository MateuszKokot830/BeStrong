using Application.Dto.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Register
{
    public class RegisterCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService) : IRequestHandler<RegisterCommand, ErrorOr<UserAuthResponseDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<ErrorOr<UserAuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existing = await _userRepository.GetByUsernameAsync(request.UserRegisterRequestDto.UserName, cancellationToken);
            if (existing is not null)
                return Errors.User.DuplicateUsername;

            var user = request.UserRegisterRequestDto.ToEntity();

            var result = await _userRepository.RegisterUserAsync(user, request.UserRegisterRequestDto.Password, cancellationToken);

            if (!result.Succeeded)
                return result.Errors
                    .Select(e => Error.Failure(code: e.Code, description: e.Description))
                    .ToList();

            var token = await _tokenService.CreateTokenAsync(user.ToDto(), cancellationToken);

            return new UserAuthResponseDto(user.UserName, token);
        }
    }
}
