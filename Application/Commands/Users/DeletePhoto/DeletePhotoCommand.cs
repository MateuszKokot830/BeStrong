using MediatR;

namespace Application.Commands.Users.DeletePhoto
{
    public class DeletePhotoCommand : IRequest<Unit>
    {
        public int PhotoId { get; set; }
        public int UserId { get; set; }
    }
}