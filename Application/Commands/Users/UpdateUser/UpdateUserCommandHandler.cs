using Application.Interfaces;
using Domain.Aggregates;
using AutoMapper;
using MediatR;
using System.Security.Claims;

namespace Application.Commands.Users.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {   
            var user = _userRepository.GetByIdAsync(request.UserUpdateDto.Id).Result;

            if (user != null) {
                _mapper.Map(request.UserUpdateDto, user);
                await _userRepository.UpdateAsync(user);
            }
       
            return Unit.Value;
        }
    }
}