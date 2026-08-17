using Application.Dto.User;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.UpdateUser
{
    public class UpdateUserCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UpdateUserCommand, ErrorOr<UserDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserUpdateDto.Id, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(user.Id))
                return Errors.User.Forbidden;

            request.UserUpdateDto.ApplyTo(user);
            user.UpdatedDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user, cancellationToken);
            return user.ToDto();
        }
    }
}
