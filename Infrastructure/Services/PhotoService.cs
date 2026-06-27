using Application.Dto.Photo;
using Application.Interfaces.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Infrastructure.Helpers;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<PhotoUploadResult> UploadAsync(Stream content, string fileName, CancellationToken cancellationToken = default)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, content),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                Folder = "bestrong"
            };

            var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

            if (result.Error != null)
                throw new InvalidOperationException(result.Error.Message);

            return new PhotoUploadResult(result.SecureUrl.AbsoluteUri, result.PublicId);
        }

        public async Task DeleteAsync(string publicId, CancellationToken cancellationToken = default)
        {
            var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));

            if (result.Error != null)
                throw new InvalidOperationException(result.Error.Message);
        }
    }
}
