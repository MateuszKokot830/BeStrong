using Application.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Users.AddPhoto
{
    public class AddPhotoCommand : IRequest
    {
        public IFormFile File  { get; set; }
        public string Username { get; set; }
    }
}