using Application.Dto.User;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Users.GetUsersByIds
{
    public class GetUsersByIdsQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUsersByIdsQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<UserDto>> Handle(GetUsersByIdsQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            var selectedUsers = users.Where(u => request.UserIds.Contains(u.Id));

            return _mapper.Map<IEnumerable<UserDto>>(selectedUsers);
        }
    }
}