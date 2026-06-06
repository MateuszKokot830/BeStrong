using AutoMapper;
using MediatR;
using Domain.Aggregates;
using Domain.Errors;
using ErrorOr;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Dto.Auth;
using Application.Dto.User;

namespace Application.Commands.Register
{
    public class RegisterCommandHandler(IUserRepository userRepository,
                                IMapper mapper,
                                ITokenService tokenService) : IRequestHandler<RegisterCommand, ErrorOr<UserAuthResponseDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<ErrorOr<UserAuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetByUsernameAsync(request.UserRegisterRequestDto.UserName) != null)
                return Errors.User.DuplicateUsername;

            var user = _mapper.Map<User>(request.UserRegisterRequestDto);

            var result = await _userRepository.RegisterUserAsync(user, request.UserRegisterRequestDto.Password);

            if (!result.Succeeded)
                return Errors.User.FailedRegister;

            var userDto = _mapper.Map<UserDto>(user);

            return new UserAuthResponseDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(userDto)
            };
        }
    }
}