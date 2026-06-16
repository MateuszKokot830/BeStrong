using Application.Dto.Photo;
using Domain.Entities;

namespace Application.Mappings
{
    public static class PhotoMappings
    {
        public static PhotoDto ToDto(this Photo photo) => new(
            photo.Id,
            photo.PublicId,
            photo.Url,
            photo.IsProfilePhoto
        );
    }
}
