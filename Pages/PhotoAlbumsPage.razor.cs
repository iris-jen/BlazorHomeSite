using BlazorHomeSite.Data;
using BlazorHomeSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BlazorHomeSite.Pages;

public partial class PhotoAlbumsPage
{
    private const int photosPerPage = 50;

    [Inject] private IPhotoService PhotoService { get; set; } = null!;
    [Inject] private ILogger<PhotoAlbumPage> Logger { get; set; } = null!;

    private List<PhotoAlbum>? PhotoAlbums { get; set; }
    private string NewAlbumDescription { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        PhotoAlbums = await PhotoService.GetPhotoAlbums();
    }

    protected async Task AddNewAlbum()
    {
        var newAlbum = new PhotoAlbum() { Description = NewAlbumDescription };
        await PhotoService.CreatePhotoAlbum(newAlbum);
        PhotoAlbums = await PhotoService.GetPhotoAlbums();
    }
}