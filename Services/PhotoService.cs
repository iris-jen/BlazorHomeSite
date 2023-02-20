using BlazorHomeSite.Data;
using BlazorHomeSite.Services.Interfaces;

namespace BlazorHomeSite.Services
{
    public class PhotoService : IPhotoService
    {
        public PhotoService(HomeSiteDbContext dbContext)
        {
        }

        public Task CreatePhotoAlbum(PhotoAlbum album)
        {
            throw new NotImplementedException();
        }

        public Task DeletePhoto(int photoID)
        {
            throw new NotImplementedException();
        }

        public Task DeletePhotoAlbum(int photoAlbumID)
        {
            throw new NotImplementedException();
        }

        public Task EditPhoto(Photo photo)
        {
            throw new NotImplementedException();
        }

        public Task EditPhotoAlbum(PhotoAlbum album)
        {
            throw new NotImplementedException();
        }

        public List<PhotoAlbum> GetPhotoAlbums()
        {
            throw new NotImplementedException();
        }

        public List<Photo> GetPhotosByAlbum(int albumID)
        {
            throw new NotImplementedException();
        }

        public Task UploadPhoto(Photo photo)
        {
            throw new NotImplementedException();
        }
    }
}