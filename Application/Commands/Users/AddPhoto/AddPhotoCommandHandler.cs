using Application.Interfaces;
using Domain.Aggregates;
using AutoMapper;
using MediatR;
using Domain.Entities;
using Application.Dto;

namespace Application.Commands.Users.AddPhoto
{
    public class AddPhotoCommandHandler : IRequestHandler<AddPhotoCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public AddPhotoCommandHandler(IUserRepository userRepository, IPhotoService photoService, IMapper mapper)
        {
            _userRepository = userRepository;
            _photoService = photoService;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {   
            var user = _userRepository.GetByIdAsync(request.UserId).Result;

            var result = await _photoService.AddPhotoAsync(request.File);

            var photo = new Photo 
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                UserId = user.Id
            };

            if (user.Photos.Count == 0) photo.IsProfilePhoto = true;

            await _userRepository.AddPhoto(photo);

            return Unit.Value;
        }
    }
}