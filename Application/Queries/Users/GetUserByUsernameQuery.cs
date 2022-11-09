using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Users
{
    public class GetUserByUsernameQuery: IRequest<UserAggregateDto>
    {
        public string Username { get; set; }

        public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, UserAggregateDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public GetUserByUsernameQueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<UserAggregateDto> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByUsername(request.Username);
                
                return _mapper.Map<UserAggregateDto>(user);
            }
        }
    }
}