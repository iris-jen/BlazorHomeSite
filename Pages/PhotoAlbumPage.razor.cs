using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;

namespace BlazorHomeSite.Pages;

public partial class PhotoAlbumPage
{
    [Parameter]
    public string? AlbumId
    {
        get => _albumId;
        set
        {
            _albumId = value;
            AllPhotos = GetAllPhotos();
            Pages = (int)System.Double.Round(AllPhotos.Count / PhotosPerPage, 0);
            PagePhotos = GetPhotosByPage(0, PhotosPerPage);
            this.StateHasChanged();
        }
    }

    private string _albumId;
    const int PhotosPerPage = 50;

    private int _page = 1;
    private int Page
    {
        get => _page;
        set
        {
            _page = value;
            PagePhotos = GetPhotosByPage(value, PhotosPerPage);
            this.StateHasChanged();
        }
    }
    private int Pages { get; set; }

    private List<Photo>? AllPhotos { get; set; }
    private List<Photo>? PagePhotos { get; set; }
    public string? AlbumDescription { get; set; }
    
    private List<Photo>? GetAllPhotos()
    {
        using var context =  DbFactory.CreateDbContext();

        AlbumDescription = context.PhotoAlbums.FirstOrDefault(x => x.Id == int.Parse(AlbumId)).Description;
        
        return context.Photos.Where(
            x => x.Album.Id == int.Parse(AlbumId))
            .OrderBy(x=>x.CaptureTime).ToList();
    }
    public List<Photo> GetPhotosByPage(int pageNumber, int itemsToTake)
    {
        return AllPhotos.OrderBy(x=>x.CaptureTime)
                        .Skip(pageNumber> 1 ? pageNumber * itemsToTake : 0)
                        .Take(itemsToTake).ToList();
    }

}