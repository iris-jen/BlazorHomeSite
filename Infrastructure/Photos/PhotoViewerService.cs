using Ardalis.Specification;
using Domain.Photos;

namespace Services.Photos
{
    public class PhotoViewerService
    {
        private readonly IReadRepositoryBase<Photo> _photoRepository;

        public PhotoViewerService(IReadRepositoryBase<Photo> photoRepository, IReadRepositoryBase<PhotoAlbum> photoAlbumRepository)
        {
        }

        public async Task<Stream?> GetImageStreamAsync(int photoId, CancellationToken token)
        {
            var photoEntity = await _photoRepository.GetByIdAsync(photoId, token);
            if (photoEntity == null)
            {
                return null;
            }

            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);

            var image = await Image.LoadAsync(photoEntity.PhotoPath, token);
            var format = await Image.DetectFormatAsync(photoEntity.PhotoPath, token);

            await writer.WriteAsync(image.ToBase64String(format));
            writer.Flush();
            stream.Position = 0;
            image.Dispose();
            return stream;
        }
    }
}