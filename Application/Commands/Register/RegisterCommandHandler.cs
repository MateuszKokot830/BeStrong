using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Domain.Aggregates;
using Domain.Errors;
using ErrorOr;

namespace Application.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<UserAuthResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly INLoggerService _loggerService;
        public RegisterCommandHandler(IUserRepository userRepository, 
                                    IMapper mapper, 
                                    ITokenService tokenService,
                                    INLoggerService loggerService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _loggerService = loggerService;
        }

        public async Task<ErrorOr<UserAuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetByUsernameAsync(request.userRegisterRequestDto.UserName) != null) 
                return Errors.User.DuplicateUsername;

            var user = _mapper.Map<UserAggregate>(request.userRegisterRequestDto);

            var result = _userRepository.RegisterUserAsync(user, request.userRegisterRequestDto.Password);
            if (!result.Result.Succeeded) return Errors.User.FailedRegister;

            var userDto = _mapper.Map<UserAggregateDto>(user);

            return new UserAuthResponseDto 
            {
                Username = user.UserName, 
                Token = _tokenService.CreateToken(userDto)
            };
        }
    }
}