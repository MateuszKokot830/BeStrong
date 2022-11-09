using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace Application.Commands.Authentication
{
    public class AuthenticateUserCommand : IRequest<bool>
    {
        public UserAggregateDto UserAggregateDto { get; set; }
        public UserLoginRequestDto UserLoginRequestDto { get; set; }

        public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, bool>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public AuthenticateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public Task<bool> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
            {
                using var hmac = new HMACSHA512(request.UserAggregateDto.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.UserLoginRequestDto.Password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != request.UserAggregateDto.PasswordHash[i]) return Task.FromResult(false);
                }

                return Task.FromResult(true);
            }
        }
    }

}