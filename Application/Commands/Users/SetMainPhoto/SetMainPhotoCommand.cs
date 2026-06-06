using MediatR;

namespace Application.Commands.Users.SetMainPhoto
{
    public record SetMainPhotoCommand(int PhotoId, int UserId) : IRequest<Unit>;
}