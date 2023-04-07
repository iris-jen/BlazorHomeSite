namespace BlazorHomeSite.Services.Photos;

public interface IPhotosService
{
    Task<Dictionary<int, string>> GetB64ImagesByPage(int albumid, int page, int photosPerPage);

    Task<int> GetPagesForAlbum(int albumId);
}