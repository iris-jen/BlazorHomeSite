using Microsoft.AspNetCore.Components.Forms;

namespace Infrastructure.Photos
{
    public interface IPhotoAdminService
    {
        Task UploadPhotos(IReadOnlyList<IBrowserFile> photos, int albumId);

        public Task DeletePhoto(int photoId);

        public Task EditPhotoDescription(string description, int photoID);

        public Task EditPhotoLocation(string location, int photoId);

        public Task EditPhotoCoOrdinates(string coOrdinates, int photoId);
    }
}