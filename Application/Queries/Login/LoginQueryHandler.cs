using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Domain.Aggregates;
using Domain.Errors;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

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
            var user = await _userRepository.GetByUsernameAsync(request.userLoginRequestDto.UserName);
            if (user == null)  return Errors.Auth.InvalidUsername;

            var result = _userRepository.CheckPasswordAsync(user, request.userLoginRequestDto.Password);
            if (!result.Result) return Errors.Auth.InvalidPassword;

            var userDto = _mapper.Map<UserAggregateDto>(user);

            return new UserAuthResponseDto 
            {
                Username = user.UserName, 
                Token = _tokenService.CreateToken(userDto)
            };
        }
    }
}