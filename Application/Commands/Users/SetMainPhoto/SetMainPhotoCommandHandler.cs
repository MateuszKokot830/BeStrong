using Application.Interfaces.Repositories;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.SetMainPhoto
{
    public class SetMainPhotoCommandHandler(IUserRepository userRepository) : IRequestHandler<SetMainPhotoCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ErrorOr<Unit>> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);
            if (photo is null)
                return Errors.Photo.NotFound;

            var mainPhoto = user.Photos.FirstOrDefault(x => x.IsProfilePhoto);
            mainPhoto?.IsProfilePhoto = false;
            photo.IsProfilePhoto = true;

            await _userRepository.UpdateAsync(user, cancellationToken);
            return Unit.Value;
        }
    }
}
