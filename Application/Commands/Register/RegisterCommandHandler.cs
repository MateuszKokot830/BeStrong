using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System.Security.Cryptography;
using System.Text;
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
        public RegisterCommandHandler(IUserRepository userRepository, 
                                    IMapper mapper, 
                                    ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<ErrorOr<UserAuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetByUsername(request.userRegisterRequestDto.Username) != null) 
                return Errors.User.DuplicateUsername;

            using var hmac = new HMACSHA512();

            var user = new UserAggregate
            {
                Username = request.userRegisterRequestDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.userRegisterRequestDto.Password)),
                PasswordSalt = hmac.Key
            };

            await _userRepository.AddAsync(user);
            var userDto = _mapper.Map<UserAggregateDto>(user);

            return new UserAuthResponseDto 
            {
                Username = user.Username, 
                Token = _tokenService.CreateToken(userDto)
            };
        }
    }
}