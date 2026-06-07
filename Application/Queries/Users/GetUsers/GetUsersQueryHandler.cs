using Application.Dto.User;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsers
{
    public class GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUsersQuery, ErrorOr<IEnumerable<UserDto>>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<IEnumerable<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository.GetAllAsync(cancellationToken);
                return _mapper.Map<IEnumerable<UserDto>>(users).OrderBy(a => a.UserName).ToList();
            }
            catch (Exception)
            {
                return Errors.User.NotFound;
            }
        }
    }
}
