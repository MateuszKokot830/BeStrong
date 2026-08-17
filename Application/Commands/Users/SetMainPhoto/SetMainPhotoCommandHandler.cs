using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.SetMainPhoto
{
    public class SetMainPhotoCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService) : IRequestHandler<SetMainPhotoCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(user.Id))
                return Errors.User.Forbidden;

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);
            if (photo is null)
                return Errors.Photo.NotFound;

            var currentMain = user.Photos.FirstOrDefault(x => x.IsProfilePhoto);
            if (currentMain is not null)
                currentMain.IsProfilePhoto = false;

            photo.IsProfilePhoto = true;

            await _userRepository.UpdateAsync(user, cancellationToken);
            return Unit.Value;
        }
    }
}
