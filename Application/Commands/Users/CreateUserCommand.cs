using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System.Security.Cryptography;
using System.Text;
using Domain.Aggregates;

namespace Application.Commands.Users
{
    public class CreateUserCommand : IRequest<UserAggregateDto>
    {
        public UserRegisterRequestDto UserRegisterRequestDto { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserAggregateDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<UserAggregateDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                using var hmac = new HMACSHA512();
                var user = new UserAggregate
                {
                    Username = request.UserRegisterRequestDto.Username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.UserRegisterRequestDto.Password)),
                    PasswordSalt = hmac.Key
                };
                await _userRepository.AddAsync(user);

                return _mapper.Map<UserAggregateDto>(user);
            }
        }
    }
}