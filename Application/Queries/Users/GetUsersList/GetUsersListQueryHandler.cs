using Application.Dto;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.Aggregates;
using MediatR;

namespace Application.Queries.Users.GetUsersList
{
    public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, PaginationList<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersListQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PaginationList<UserDto>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetUsersAsync(request.PaginationParams);
            return _mapper.Map<PaginationList<UserDto>>(users);
        }
    }
}