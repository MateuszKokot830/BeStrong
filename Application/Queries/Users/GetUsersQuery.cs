using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Queries.Users
{
    public class GetUsersQuery : IRequest<IEnumerable<UserAggregateDto>>
    {
        public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserAggregateDto>>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<UserAggregateDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                var users = await _userRepository.GetAllAsync();
                users.OrderBy(a => a.UserName);

                return _mapper.Map<IEnumerable<UserAggregateDto>>(users);
            }
        }
    }
}