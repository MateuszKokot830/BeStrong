using Application.Dto;
using Application.Helpers;
using Domain.Aggregates;
using MediatR;

namespace Application.Queries.Users.GetUsersList
{
    public class GetUsersListQuery : IRequest<PaginationList<UserDto>>
    {
        public PaginationParams PaginationParams { get; set; }
    }
}