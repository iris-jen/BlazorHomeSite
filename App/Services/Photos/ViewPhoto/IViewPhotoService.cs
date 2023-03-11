namespace BlazorHomeSite.Services.Photos;

public interface IViewPhotoService
{
    Task<Stream?> GetImageStreamAsync();
}