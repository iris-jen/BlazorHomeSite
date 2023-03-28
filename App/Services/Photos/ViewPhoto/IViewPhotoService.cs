namespace BlazorHomeSite.Services.Photos;

public interface IViewPhotoService
{
    string GetImageB64(int photoId, bool thumbnail, string path);
}