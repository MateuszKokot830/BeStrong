using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Users.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            users.OrderBy(a => a.UserName);

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}