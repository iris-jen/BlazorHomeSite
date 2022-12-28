using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Pages;

public partial class PhotoPage
{
    [Inject] private IDbContextFactory<ApplicationDbContext>? DbFactory { get; set; }

    #region Parameters
   
    [Parameter]
    public string? AlbumId { get; set; }
    
    [Parameter]
    public string? PhotoId 
    {
        get => _photoId;
        set
        {
            _photoId = value;
            GetAllPhotos(int.Parse(_photoId), int.Parse(AlbumId));
        }
    }
    private string _photoId { get; set; }
    
    #endregion

    #region Photo Binding
    private int CurrentPhotoIndex { get; set; }
    private Photo CurrentPhoto { get; set; }
    
    private SortedList<int, Photo> Photos = new SortedList<int, Photo>();
    

    private string? AlbumDescription { get; set; }
    
    #endregion Photo Binding

    #region Information Editor Binding
    public bool EditPhotoInformation { get; set; }
    public string PhotoLocation { get; set; }
    public string PhotoCoOrdinates { get; set; }
    public string PhotoDescription { get; set; }
    public bool IsAlbumCover { get; set; }
    
    #endregion

    
    private SortedList<int, Photo>? GetAllPhotos(int photoId, int albumId)
    {
        List<Photo>? photos = null; 
        if (DbFactory != null)
            photos = DataHelper.GetPhotoAlbum(DbFactory, albumId).Photos;

        SortedList<int, Photo>? outputPhotos = null;
        var oderedPhotos = photos.OrderBy(x => x.CaptureTime).ToArray();
        
        for (var i= 0; i < oderedPhotos.Count() ; i++)
        {
            outputPhotos.Add(i, oderedPhotos[i]);

            if (oderedPhotos[i].Id == photoId)
            {
                CurrentPhoto = oderedPhotos[i];
                CurrentPhotoIndex = i;
            }
        }
        return outputPhotos;
    }
    
    private bool EnableEdit()
    {
#if DEBUG
        return true;
#else
                    return false;
#endif
    }


    private void NextPhoto()
    {
        CurrentPhotoIndex++;
        CurrentPhoto = Photos[CurrentPhotoIndex];
    }

    private void LastPhoto()
    {
        CurrentPhotoIndex--;
        CurrentPhoto = Photos[CurrentPhotoIndex];
    }

    private void UpdatePhoto()
    {
        CurrentPhoto.Description = PhotoDescription;
        CurrentPhoto.Location = PhotoLocation;
        CurrentPhoto.LocationCoOrdinates = PhotoCoOrdinates;
        CurrentPhoto.IsAlbumCover = IsAlbumCover;
        EditPhotoInformation = false;
        StateHasChanged();
    }

    private void SetEdit(bool edit)
    {
        if (edit)
        {
            CurrentPhoto.Description = PhotoDescription;
            CurrentPhoto.Location = PhotoLocation;
            CurrentPhoto.LocationCoOrdinates = PhotoCoOrdinates;
            CurrentPhoto.IsAlbumCover = IsAlbumCover;    
        }
        
        EditPhotoInformation = edit;
        StateHasChanged();
    }
}