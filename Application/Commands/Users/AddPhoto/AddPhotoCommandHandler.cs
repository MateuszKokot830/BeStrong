using AutoMapper;
using CloudinaryDotNet.Actions;
using MediatR;
using Domain.Entities;
using Domain.Errors;
using ErrorOr;
using Application.Dto.Photo;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.Commands.Users.AddPhoto
{
    public class AddPhotoCommandHandler(IUserRepository userRepository, IPhotoService photoService, IMapper mapper) : IRequestHandler<AddPhotoCommand, ErrorOr<PhotoDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPhotoService _photoService = photoService;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<PhotoDto>> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            ImageUploadResult result;
            try
            {
                result = await _photoService.AddPhotoAsync(request.File);
            }
            catch (Exception)
            {
                return Errors.Photo.UploadFailed;
            }

            if (result.Error != null)
                return Errors.Photo.UploadFailed;

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                UserId = user.Id
            };

            if (user.Photos.Count == 0)
                photo.IsProfilePhoto = true;

            await _userRepository.AddPhoto(photo, cancellationToken);
            return _mapper.Map<PhotoDto>(photo);
        }
    }
}
