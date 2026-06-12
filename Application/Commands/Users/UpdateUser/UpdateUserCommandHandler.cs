using Application.Dto.User;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.UpdateUser
{
    public class UpdateUserCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IMapper mapper) : IRequestHandler<UpdateUserCommand, ErrorOr<UserDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserUpdateDto.Id, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(user.Id))
                return Errors.User.Unauthorized;

            _mapper.Map(request.UserUpdateDto, user);
            await _userRepository.UpdateAsync(user, cancellationToken);
            return _mapper.Map<UserDto>(user);
        }
    }
}
