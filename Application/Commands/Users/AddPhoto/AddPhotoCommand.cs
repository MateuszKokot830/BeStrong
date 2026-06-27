using Application.Dto.Photo;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.AddPhoto
{
    public record AddPhotoCommand(Stream Content, string FileName, long Length, int UserId) : IRequest<ErrorOr<PhotoDto>>;
}
