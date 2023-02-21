using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Music;
using BlazorHomeSite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IDbContextFactory<HomeSiteDbContext> _dbFactory;

        public PhotoService(IDbContextFactory<HomeSiteDbContext> DbFactory)
        {
            _dbFactory = DbFactory;
        }

        #region Photo Albums

        public async Task CreatePhotoAlbum(PhotoAlbum album)
        {
            var context = await _dbFactory.CreateDbContextAsync();
            await context.PhotoAlbums.AddAsync(album);
            await context.SaveChangesAsync();
        }

        public async Task DeletePhotoAlbum(int photoAlbumID)
        {
            var context = await _dbFactory.CreateDbContextAsync();
            await context.PhotoAlbums.Where(x => x.Id == photoAlbumID).ExecuteDeleteAsync();
            await context.SaveChangesAsync();
        }

        public Task EditPhotoAlbum(PhotoAlbum album)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PhotoAlbum>> GetPhotoAlbums()
        {
            var context = await _dbFactory.CreateDbContextAsync();
            return await context.PhotoAlbums.ToListAsync();
        }

        #endregion Photo Albums

        #region Photos

        public async Task DeletePhoto(int photoID)
        {
            var context = await _dbFactory.CreateDbContextAsync();
            await context.Photos.Where(x => x.Id == photoID).ExecuteDeleteAsync();
            await context.SaveChangesAsync();
        }

        public async Task EditPhoto(Photo photo)
        {
            var context = await _dbFactory.CreateDbContextAsync();
            context.Photos.Update(photo);
            await context.SaveChangesAsync();
        }

        public Task UploadPhoto(Photo photo)
        {
            throw new NotImplementedException();
        }

        Task<List<Photo>> IPhotoService.GetPhotosByAlbum(int albumID)
        {
            throw new NotImplementedException();
        }

        #endregion Photos
    }
}