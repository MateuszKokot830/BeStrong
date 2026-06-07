using AutoMapper;
using MediatR;
using Application.Interfaces.Repositories;
using Application.Dto.User;
using Domain.Errors;
using ErrorOr;

namespace Application.Queries.Users.GetUserByUsername
{
    public class GetUserByUsernameQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserByUsernameQuery, ErrorOr<UserDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<UserDto>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            return _mapper.Map<UserDto>(user);
        }
    }
}
