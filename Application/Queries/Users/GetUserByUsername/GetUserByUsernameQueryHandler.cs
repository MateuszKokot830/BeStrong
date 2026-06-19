using Application.Dto.User;
using Application.Interfaces.Searchers;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUserByUsername
{
    public class GetUserByUsernameQueryHandler(IUserSearcher userSearcher)
        : IRequestHandler<GetUserByUsernameQuery, ErrorOr<UserDto>>
    {
        private readonly IUserSearcher _userSearcher = userSearcher;

        public async Task<ErrorOr<UserDto>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userSearcher.FindByUsernameAsync(request.Username, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            return user;
        }
    }
}
