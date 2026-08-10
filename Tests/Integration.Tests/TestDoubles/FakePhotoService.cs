using Application.Dto.Photo;
using Application.Interfaces.Services;

namespace Integration.Tests.TestDoubles
{
    // Cloudinary's real client can't be swapped in via configuration (PhotoService constructs it
    // directly from IOptions<CloudinarySettings>), so integration tests substitute this fake at the
    // DI level instead — the same way TestWebApplicationFactory swaps DataContext for Sqlite.
    internal sealed class FakePhotoService : IPhotoService
    {
        private int _uploadCount;

        public Task<PhotoUploadResult> UploadAsync(Stream content, string fileName, CancellationToken cancellationToken = default)
        {
            var id = Interlocked.Increment(ref _uploadCount);
            return Task.FromResult(new PhotoUploadResult($"https://fake-cdn.test/{id}/{fileName}", $"fake-public-id-{id}"));
        }

        public Task DeleteAsync(string publicId, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }
}
