using Application.Dto;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.DeletePhoto
{
    public class DeletePhotoCommand : IRequest <ErrorOr<Unit>>
    {
        public int PhotoId { get; set; }
        public int UserId { get; set; }
    }
}