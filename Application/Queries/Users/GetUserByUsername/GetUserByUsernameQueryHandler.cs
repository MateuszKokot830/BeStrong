using AutoMapper;
using MediatR;
using Application.Interfaces.Repositories;
using Application.Dto.User;

namespace Application.Queries.Users.GetUserByUsername
{
    public class GetUserByUsernameQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserByUsernameQuery, UserDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<UserDto> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            return _mapper.Map<UserDto>(user);
        }
    }
}