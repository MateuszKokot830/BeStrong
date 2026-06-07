using ErrorOr;
using MediatR;

namespace Application.Commands.Users.DeletePhoto
{
    public record DeletePhotoCommand(int PhotoId, int UserId) : IRequest<ErrorOr<Unit>>;
}
