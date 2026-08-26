using Application.Dto.User;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.UpdateUserSettings
{
    public class UpdateUserSettingsCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UpdateUserSettingsCommand, ErrorOr<UserSettingsDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<UserSettingsDto>> Handle(UpdateUserSettingsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_currentUserService.UserId, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            user.Settings = request.SettingsDto.ToEntity();
            await _userRepository.UpdateAsync(user, cancellationToken);
            return user.Settings.ToDto();
        }
    }
}
