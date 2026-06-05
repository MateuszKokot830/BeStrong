using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Commands.Users.SetMainPhoto
{
    public class SetMainPhotoCommandHandler : IRequestHandler<SetMainPhotoCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public SetMainPhotoCommandHandler(IUserRepository userRepository, IPhotoService photoService, IMapper mapper)
        {
            _userRepository = userRepository;
            _photoService = photoService;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
        {   
            var user = _userRepository.GetByIdAsync(request.UserId).Result;

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);
            var mainPhoto = user.Photos.FirstOrDefault(x => x.IsProfilePhoto);

            if (mainPhoto != null) mainPhoto.IsProfilePhoto = false;
            photo.IsProfilePhoto = true;

            await _userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}