using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;


namespace Application.Queries.Users
{
    public class GetUserByIdQuery : IRequest<UserAggregateDto>
    {
        public int Id { get; set; }

        public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserAggregateDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<UserAggregateDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(request.Id);

                return _mapper.Map<UserAggregateDto>(user);
            }
        }
    }
}