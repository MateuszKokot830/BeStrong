using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System.Security.Cryptography;
using System.Text;
using Domain.Aggregates;
using Domain.Errors;
using ErrorOr;

namespace Application.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<UserAuthResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public LoginQueryHandler(IUserRepository userRepository, 
                                IMapper mapper, 
                                ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<ErrorOr<UserAuthResponseDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsername(request.userLoginRequestDto.Username);
            if (user == null)  return Errors.Auth.InvalidUsername;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.userLoginRequestDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Errors.Auth.InvalidPassword;
            }

            var userDto = _mapper.Map<UserAggregateDto>(user);

            return new UserAuthResponseDto 
            {
                Username = user.Username, 
                Token = _tokenService.CreateToken(userDto)
            };
        }
    }
}