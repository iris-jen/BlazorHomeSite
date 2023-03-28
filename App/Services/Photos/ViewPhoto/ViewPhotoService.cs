using BlazorHomeSite.Data.Domain;
using BlazorHomeSite.Services.Database;
using LazyCache;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BlazorHomeSite.Services.Photos;

public class ViewPhotoService : IViewPhotoService
{
    private readonly IAppCache _cache;

    public ViewPhotoService(IAppCache cache)
    {
        _cache = cache;
    }

    public string GetImageB64(int photoId, bool thumbnail, string path)
    {
        var cacheKey = $"photo-{photoId}-t{thumbnail}";

        var result = _cache.GetOrAdd<string>(cacheKey, () => ReadB64FromFile(thumbnail, path));
        return result;
    }

    private string ReadB64FromFile(bool thumbnail, string path)
    {
        var document = File.ReadAllText(path);
        var b64Model = JsonConvert.DeserializeObject<B64ImageStorage>(document);
        return thumbnail ? b64Model.Thumbnail : b64Model.Regular;
    }
}