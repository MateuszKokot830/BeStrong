using AutoMapper;
using MediatR;
using Domain.Errors;
using ErrorOr;
using Application.Dto.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Dto.User;

namespace Application.Queries.Login
{
    public class LoginQueryHandler(IUserRepository userRepository,
                            IMapper mapper,
                            ITokenService tokenService) : IRequestHandler<LoginQuery, ErrorOr<UserAuthResponseDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<ErrorOr<UserAuthResponseDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.UserLoginRequestDto.UserName, cancellationToken);

            if (user == null)
                return Errors.Auth.InvalidUsername;

            var result = await _userRepository.CheckPasswordAsync(user, request.UserLoginRequestDto.Password, cancellationToken);

            if (!result)
                return Errors.Auth.InvalidPassword;

            var userDto = _mapper.Map<UserDto>(user);
            var token = await _tokenService.CreateTokenAsync(userDto, cancellationToken);

            return new UserAuthResponseDto(user.UserName, token);
        }
    }
}