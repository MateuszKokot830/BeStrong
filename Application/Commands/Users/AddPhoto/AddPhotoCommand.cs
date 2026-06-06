using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Users.AddPhoto
{
    public record AddPhotoCommand(IFormFile File, int UserId) : IRequest;
}