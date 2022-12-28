using BlazorHomeSite.Data;
using BlazorHomeSite.Migrations;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Pages;

public partial class PhotoAlbumsPage
{
    [Inject] private IDbContextFactory<ApplicationDbContext>? DbFactory { get; set; }

    private Dictionary<int, string>? AlbumCovers { get; set; }


    private string GetAlbumRoute(int id)
    {
        return $"/photoAlbum/{id}";
    }

    private List<PhotoAlbum> GetAllAlbums()
    {


        return new List<PhotoAlbum>();
    }
}