using Application.Dto.User;
using Application.Interfaces.Repositories;
using Application.Mappings;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUserByUsername
{
    public class GetUserByUsernameQueryHandler(IUserRepository userRepository)
        : IRequestHandler<GetUserByUsernameQuery, ErrorOr<UserDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ErrorOr<UserDto>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            return user.ToDto();
        }
    }
}
