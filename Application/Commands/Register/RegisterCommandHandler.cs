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
            if (await _userRepository.GetByUsernameAsync(request.UserRegisterRequestDto.UserName, cancellationToken) != null)
                return Errors.User.DuplicateUsername;

            var user = _mapper.Map<User>(request.UserRegisterRequestDto);

            var result = await _userRepository.RegisterUserAsync(user, request.UserRegisterRequestDto.Password, cancellationToken);

            if (!result.Succeeded)
                return result.Errors
                    .Select(e => Error.Failure(code: e.Code, description: e.Description))
                    .ToList();

            var userDto = _mapper.Map<UserDto>(user);
            var token = await _tokenService.CreateTokenAsync(userDto, cancellationToken);

            return new UserAuthResponseDto(user.UserName, token);
        }
    }
}