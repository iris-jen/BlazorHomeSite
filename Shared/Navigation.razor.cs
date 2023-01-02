using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Shared;

public partial class Navigation
{
    [Inject] private IDbContextFactory<ApplicationDbContext>? DbFactory { get; set; }

    private string GetAlbumRoute(int id)
    {
        return $"/photoAlbum/{id}";
    }

    private string? GetThumbnailForAlbum(int id)
    {
        using var context = DbFactory.CreateDbContext();
        var cover = context.Photos.FirstOrDefault(x => x.AlbumId == id && x.IsAlbumCover);
        return cover?.ThumbnailPath;
    }

    private List<PhotoAlbum> GetAllAlbums()
    {
        if (DbFactory == null) return new List<PhotoAlbum>();

        using var context = DbFactory.CreateDbContext();
        var albums = context.PhotoAlbums.Where(x => x.Description != "All The Rest").ToList();
        return albums;
    }
}