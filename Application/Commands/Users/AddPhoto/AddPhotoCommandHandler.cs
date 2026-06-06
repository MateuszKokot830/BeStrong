using MediatR;
using Domain.Entities;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.Commands.Users.AddPhoto
{
    public class AddPhotoCommandHandler(IUserRepository userRepository, IPhotoService photoService) : IRequestHandler<AddPhotoCommand>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPhotoService _photoService = photoService;

        public async Task<Unit> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetByIdAsync(request.UserId).Result;

            if (user is null)
                return Unit.Value;

            var result = await _photoService.AddPhotoAsync(request.File);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                UserId = user.Id
            };

            if (user.Photos.Count == 0)
                photo.IsProfilePhoto = true;

            await _userRepository.AddPhoto(photo);

            return Unit.Value;
        }
    }
}