using Application.Dto.Photo;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Entities;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.AddPhoto
{
    public class AddPhotoCommandHandler(
        IUserRepository userRepository,
        IPhotoService photoService,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork) : IRequestHandler<AddPhotoCommand, ErrorOr<PhotoDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPhotoService _photoService = photoService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<PhotoDto>> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(user.Id))
                return Errors.User.Forbidden;

            PhotoUploadResult upload;
            try
            {
                upload = await _photoService.UploadAsync(request.Content, request.FileName, cancellationToken);
            }
            catch (Exception)
            {
                return Errors.Photo.UploadFailed;
            }

            var photo = new Photo
            {
                Url = upload.Url,
                PublicId = upload.PublicId,
                UserId = user.Id,
                IsProfilePhoto = user.Photos.Count == 0
            };

            await _userRepository.AddPhotoAsync(photo, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return photo.ToDto();
        }
    }
}
