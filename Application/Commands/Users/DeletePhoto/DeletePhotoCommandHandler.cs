using Application.Interfaces;
using MediatR;

namespace Application.Commands.Users.DeletePhoto
{
    public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;

        public DeletePhotoCommandHandler(IUserRepository userRepository, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _photoService = photoService;
        }

        public async Task<Unit> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {   
            var user = _userRepository.GetByIdAsync(request.UserId).Result;
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