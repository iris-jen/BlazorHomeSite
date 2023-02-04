using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Shared;

public partial class Navigation
{
    [Inject] private IDbContextFactory<HomeSiteDbContext>? DbFactory { get; set; }

    private static string GetAlbumRoute(int id)
    {
        return $"/photoAlbum/{id}";
    }

    private string? GetThumbnailForAlbum(int id)
    {
        if (DbFactory != null)
        {
            using var context = DbFactory.CreateDbContext();
            var cover = context.Photos.FirstOrDefault(x => x.AlbumId == id && x.IsAlbumCover);
            return cover?.ThumbnailPath;
        }

        return string.Empty;
    }

    private List<PhotoAlbum> GetAllAlbums()
    {
        if (DbFactory == null) return new List<PhotoAlbum>();

        using var context = DbFactory.CreateDbContext();
        var albums = context.PhotoAlbums.Where(x => x.Description != "All The Rest").ToList();
        return albums;
    }
}