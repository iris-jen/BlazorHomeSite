namespace BlazorHomeSite.Services.Photos;

public interface IViewPhotoService
{
    Task<string> GetImageB64(int photoId, bool thumbnail);
}