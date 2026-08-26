using Application.Dto.User;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUserSettings
{
    public record GetUserSettingsQuery : IRequest<ErrorOr<UserSettingsDto>>;
}
