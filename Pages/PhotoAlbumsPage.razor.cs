using BlazorHomeSite.Data;
using BlazorHomeSite.Migrations;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Pages;

public partial class PhotoAlbumsPage
{
    [Inject]
    private IDbContextFactory<ApplicationDbContext>? DbFactory { get; set; }
    private Dictionary<int, string> AlbumCovers { get; set; }

    private bool EnableAdminControl()
    {
        #if DEBUG 
            return true;
        #else
            return false;
        #endif
    }
    //C:\Users\kaj\Documents\GitHub\BlazorHomeSite\wwwroot
    
    private string GetAlbumRoute(int id) => $"/photoAlbum/{id}";
    
    private List<PhotoAlbum> GetAllAlbums()
    {
        using var context = DbFactory.CreateDbContext();
        var albums = context.PhotoAlbums.ToList();
        AlbumCovers = new Dictionary<int, string>();
        
        foreach (var album in albums)
        {
            var firstPhoto = context.Photos.First(x => x.Album.Id == album.Id);
            AlbumCovers.Add(album.Id, firstPhoto.PhotoPath);
        }
        
        return albums;
    }
}