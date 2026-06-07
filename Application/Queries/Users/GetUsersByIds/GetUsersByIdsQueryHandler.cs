using Application.Dto.User;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsersByIds
{
    public class GetUsersByIdsQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUsersByIdsQuery, ErrorOr<IEnumerable<UserDto>>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<IEnumerable<UserDto>>> Handle(GetUsersByIdsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository.GetAllAsync(cancellationToken);
                var selectedUsers = users.Where(u => request.UserIds.Contains(u.Id));
                return _mapper.Map<IEnumerable<UserDto>>(selectedUsers).ToList();
            }
            catch (Exception)
            {
                return Errors.User.NotFound;
            }
        }
    }
}
