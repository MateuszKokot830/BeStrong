using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using MediatR;

namespace Application.Commands.Users.DeletePhoto
{
    public class DeletePhotoCommandHandler(IUserRepository userRepository, IPhotoService photoService) : IRequestHandler<DeletePhotoCommand, Unit>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPhotoService _photoService = photoService;

        public async Task<Unit> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null || user.Photos is null)
                return Unit.Value;

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);

            if (photo != null && !photo.IsProfilePhoto)
            {
                if (photo.PublicId != null)
                    await _photoService.DeletePhotoAsync(photo.PublicId);

                await _userRepository.DeletePhoto(photo);
            }

            return Unit.Value;
        }
    }
}