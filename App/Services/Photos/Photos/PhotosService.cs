using BlazorHomeSite.Data.Photos;
using BlazorHomeSite.Services.Database;
using LazyCache;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace BlazorHomeSite.Services.Photos;

public class PhotosService : IPhotosService
{
    private readonly IAppCache _cache;
    private readonly IDatabaseService _db;

    public PhotosService(IAppCache cache, IDatabaseService db)
    {
        _cache = cache;
        _db = db;
    }

    public async Task<Dictionary<int, string>> GetB64ImagesByPage(int albumid, int page, int photosPerPage)
    {
        var photos = await _db.Photos
                              .Where(x => x.AlbumId == albumid)
                              .OrderBy(x => x.Id).Skip(page > 1 ? photosPerPage * (page - 1) : 0)
                              .Take(photosPerPage)
                              .ToListAsync();

        var b64Output = new ConcurrentDictionary<int, string>();

        await Parallel.ForEachAsync(photos, async (photo, _) =>
        {
            var b64 = await GetImageB64(photo.Id, true, photo.PhotoPath);
            b64Output.TryAdd(photo.Id, b64);
        });

        return b64Output.OrderBy(x => x.Key).ToDictionary(x => x.Key, y => y.Value);
    }

    public async Task<int> GetPagesForAlbum(int albumId)
    {
        return await _db.Photos.Where(x => x.AlbumId == albumId).CountAsync();
    }

    public virtual async Task<string> GetImageB64(int photoId, bool thumbnail, string path)
    {
        var cacheKey = $"photo-{photoId}-t{thumbnail}";

        var result = await _cache.GetOrAddAsync(cacheKey, async () => await ReadB64FromFile(thumbnail, path));
        return result;
    }

    public virtual async Task<string> ReadB64FromFile(bool thumbnail, string path)
    {
        var document = await File.ReadAllTextAsync(path);
        var b64Model = JsonConvert.DeserializeObject<B64ImageStorage>(document);
        return thumbnail ? b64Model.Thumbnail : b64Model.Regular;
    }
}