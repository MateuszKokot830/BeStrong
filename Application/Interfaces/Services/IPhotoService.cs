using Application.Dto.Photo;

namespace Application.Interfaces.Services
{
    public interface IPhotoService
    {
        Task<PhotoUploadResult> UploadAsync(Stream content, string fileName, CancellationToken cancellationToken = default);
        Task DeleteAsync(string publicId, CancellationToken cancellationToken = default);
    }
}
