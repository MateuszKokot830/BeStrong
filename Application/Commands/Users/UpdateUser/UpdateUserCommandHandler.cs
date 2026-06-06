using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Commands.Users.UpdateUser
{
    public class UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetByIdAsync(request.UserUpdateDto.Id).Result;

            if (user != null)
            {
                _mapper.Map(request.UserUpdateDto, user);
                await _userRepository.UpdateAsync(user);
            }

            return Unit.Value;
        }
    }
}