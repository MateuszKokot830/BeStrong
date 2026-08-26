using Application.Dto.User;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.UpdateUserSettings
{
    public record UpdateUserSettingsCommand(UserSettingsDto SettingsDto) : IRequest<ErrorOr<UserSettingsDto>>;
}
