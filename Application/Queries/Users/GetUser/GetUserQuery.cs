using Application.Dto;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUser
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
}