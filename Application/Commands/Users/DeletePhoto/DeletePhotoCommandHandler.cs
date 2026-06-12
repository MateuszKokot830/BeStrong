using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.DeletePhoto
{
    public class DeletePhotoCommandHandler(
        IUserRepository userRepository,
        IPhotoService photoService,
        ICurrentUserService currentUserService) : IRequestHandler<DeletePhotoCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPhotoService _photoService = photoService;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(user.Id))
                return Errors.User.Unauthorized;

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);
            if (photo is null)
                return Errors.Photo.NotFound;

            if (photo.IsProfilePhoto)
                return Errors.Photo.IsProfilePhoto;

            if (photo.PublicId != null)
                await _photoService.DeletePhotoAsync(photo.PublicId);

            await _userRepository.DeletePhoto(photo, cancellationToken);
            return Unit.Value;
        }
    }
}
