using Ardalis.Specification;
using BlazorHomeSite.Data.Domain;
using BlazorHomeSite.Services.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BlazorHomeSite.Services.Photos;

public class ViewPhotoService : IViewPhotoService
{
    private readonly IDatabaseService _databaseService;

    public ViewPhotoService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<string> GetImageB64(int photoId, bool thumbnail)
    {
        var photoEntity = await _databaseService.Photos.FirstOrDefaultAsync(x => x.Id == photoId);
        if (photoEntity == null)
        {
            return string.Empty;
        }

        var document = await File.ReadAllTextAsync(photoEntity.PhotoPath);
        var b64Model = JsonConvert.DeserializeObject<B64ImageStorage>(document);
        return thumbnail ? b64Model.Thumbnail : b64Model.Regular;
    }
}