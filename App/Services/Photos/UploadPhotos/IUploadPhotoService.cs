using Ardalis.Result;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorHomeSite.Services.Photos;

public interface IUploadPhotoService
{
    public Task UploadPhotos(IReadOnlyList<IBrowserFile> photos, int albumId, int maxSizeBytes = 9999999);
}