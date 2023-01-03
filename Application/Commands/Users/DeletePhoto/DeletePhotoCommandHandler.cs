using Application.Interfaces;
using Domain.Aggregates;
using AutoMapper;
using MediatR;
using Domain.Entities;
using Application.Dto;
using ErrorOr;


namespace Application.Commands.Users.DeletePhoto
{
    public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public DeletePhotoCommandHandler(IUserRepository userRepository, IPhotoService photoService, IMapper mapper)
        {
            _userRepository = userRepository;
            _photoService = photoService;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Unit>> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {   
            var user = _userRepository.GetByIdAsync(request.UserId).Result;

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);

            if (!photo.IsProfilePhoto) 
            {
                if (photo.PublicId != null) await _photoService.DeletePhotoAsync(photo.PublicId);
                await _userRepository.DeletePhoto(photo);
            }

            return Unit.Value;
        }
    }
}