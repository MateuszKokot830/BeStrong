using Application.Dto;
using Application.Interfaces;
using Domain.Aggregates;
using AutoMapper;
using MediatR;

namespace Application.Queries.Users.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetUserQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {   
            var user = await _userRepository.GetByIdAsync(request.Id);

            return _mapper.Map<UserDto>(user);
        }
    }
}