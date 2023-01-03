using Application.Dto;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.SetMainPhoto
{
    public class SetMainPhotoCommand : IRequest <ErrorOr<Unit>>
    {
        public int PhotoId { get; set; }
        public int UserId { get; set; }
    }
}