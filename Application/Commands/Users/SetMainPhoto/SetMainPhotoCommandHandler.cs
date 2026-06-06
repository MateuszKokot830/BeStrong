using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Commands.Users.SetMainPhoto
{
    public class SetMainPhotoCommandHandler(IUserRepository userRepository) : IRequestHandler<SetMainPhotoCommand, Unit>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Unit> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetByIdAsync(request.UserId).Result;

            if (user is null)
                return Unit.Value;

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);
            var mainPhoto = user.Photos.FirstOrDefault(x => x.IsProfilePhoto);

            mainPhoto?.IsProfilePhoto = false;
            photo?.IsProfilePhoto = true;

            await _userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}