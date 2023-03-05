using Microsoft.AspNetCore.Components.Forms;

namespace Services.PhotoAlbums
{
    internal interface IPhotoAlbumAdminService
    {
        Task CreatePhotoAlbum(string description);

        Task EditPhotoAlbumDescription(string description);

        Task DeletePhotoAlbum(int photoAlbumId);

        Task UploadPhotos(IReadOnlyList<IBrowserFile> photos, int albumId);
    }
}