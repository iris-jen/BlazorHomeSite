using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Pages;

public partial class PhotoPage
{
    [Inject] private IDbContextFactory<ApplicationDbContext>? DbFactory { get; set; }
    [Inject] private IWebHostEnvironment HostEnvironment { get; set; } = null!;

    private SortedList<int, Photo>? GetAllPhotos(int photoId, int albumId, bool getByYear = false)
    {
        List<Photo>? photos = null;
        if (DbFactory != null)
        {
            if (getByYear)
            {
                using var context = DbFactory.CreateDbContext();
                photos = context.Photos.Where(x => x.CaptureTime.Year == albumId)
                    .OrderBy(x => x.CaptureTime).ToList();
            }
            else
            {
                photos = DataHelper.GetPhotoAlbum(DbFactory, albumId).Photos;
            }
        }


        var outputPhotos = new SortedList<int, Photo>();
        var oderedPhotos = photos.OrderBy(x => x.CaptureTime).ToArray();

        for (var i = 0; i < oderedPhotos.Count(); i++)
        {
            outputPhotos.Add(i, oderedPhotos[i]);

            if (oderedPhotos[i].Id == photoId)
            {
                CurrentPhoto = oderedPhotos[i];
                IsAlbumCover = CurrentPhoto.IsAlbumCover;
                PhotoDescription = CurrentPhoto.Description;
                photoCaptureDate = CurrentPhoto.CaptureTime;
                PhotoLocation = CurrentPhoto.Location;
                CurrentPhotoIndex = i;
            }
        }


        SetNextAndPreviousPhotos(CurrentPhotoIndex, outputPhotos);

        return outputPhotos;
    }

    private void SetNextAndPreviousPhotos(int currentIndex, SortedList<int, Photo> photos, int nextPhotoLimit = 3)
    {
        NextPhotos.Clear();
        LastPhotos.Clear();
        for (var i = currentIndex + 1; i < currentIndex + nextPhotoLimit + 1 && i <= photos.Count-1; i++)
            NextPhotos.Add(i, photos[i]);

        for (var i = currentIndex - 1; i > currentIndex - nextPhotoLimit - 1 && i >= 0; i--)
            LastPhotos.Add(i, photos[i]);
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
        if (CurrentPhotoIndex == Photos.Count - 1) return;
        CurrentPhotoIndex++;
        
        CurrentPhoto = Photos[CurrentPhotoIndex];
        IsAlbumCover = CurrentPhoto.IsAlbumCover;
        PhotoDescription = CurrentPhoto.Description;
        photoCaptureDate = CurrentPhoto.CaptureTime;
        PhotoLocation = CurrentPhoto.Location;
        SetNextAndPreviousPhotos(CurrentPhotoIndex, Photos);
        StateHasChanged();

    }

    private void LastPhoto()
    {
        if (CurrentPhotoIndex == 0) return;
        CurrentPhotoIndex--;
        CurrentPhoto = Photos[CurrentPhotoIndex];
        IsAlbumCover = CurrentPhoto.IsAlbumCover;
        PhotoDescription = CurrentPhoto.Description;
        photoCaptureDate = CurrentPhoto.CaptureTime;
        PhotoLocation = CurrentPhoto.Location;

        SetNextAndPreviousPhotos(CurrentPhotoIndex, Photos);
        StateHasChanged();
    }

    private void UpdatePhoto()
    {
        CurrentPhoto.Description = PhotoDescription;
        CurrentPhoto.Location = PhotoLocation;
        CurrentPhoto.LocationCoOrdinates = PhotoCoOrdinates;
        CurrentPhoto.IsAlbumCover = IsAlbumCover;
        CurrentPhoto.CaptureTime = photoCaptureDate.Value;
        EditPhotoInformation = false;
        StateHasChanged();

        using var context = DbFactory.CreateDbContext();
        context.Update(CurrentPhoto);
        context.SaveChanges();
    }

    private void DeletePhoto()
    {
        using var context = DbFactory.CreateDbContext();
        var id = CurrentPhoto.Id;
        var thumbnailPath = CurrentPhoto.ThumbnailPath;
        var photoPath = CurrentPhoto.PhotoPath;

        var fullThumbPath = Path.Combine(HostEnvironment.WebRootPath, thumbnailPath);
        var fullPhotoPath = Path.Combine(HostEnvironment.WebRootPath, photoPath);
        NextPhoto();

        context.Photos.Remove(new Photo { Id = id });
        File.Delete(fullThumbPath);
        File.Delete(fullPhotoPath);
        context.SaveChanges();
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

    #region Parameters

    [Parameter] public string? AlbumId { get; set; }

    [Parameter]
    public string? PhotoId
    {
        get => _photoId;
        set
        {
            _photoId = value;
            if (AlbumId.StartsWith("year_"))
            {
                var year = AlbumId.Split('_')[1];
                Photos = GetAllPhotos(int.Parse(_photoId), int.Parse(year), true);
            }
            else
            {
                Photos = GetAllPhotos(int.Parse(_photoId), int.Parse(AlbumId));
            }
        }
    }

    private string _photoId { get; set; }

    #endregion

    #region Photo Binding

    private int CurrentPhotoIndex { get; set; }
    private Photo CurrentPhoto { get; set; }

    private SortedList<int, Photo> Photos = new();

    private readonly SortedList<int, Photo> NextPhotos = new();
    private readonly SortedList<int, Photo> LastPhotos = new();

    private string? AlbumDescription { get; set; }

    #endregion Photo Binding

    #region Information Editor Binding

    public bool EditPhotoInformation { get; set; }
    public string PhotoLocation { get; set; }
    public string PhotoCoOrdinates { get; set; }
    public string PhotoDescription { get; set; }

    private DateTime? photoCaptureDate;
    public bool IsAlbumCover { get; set; }

    #endregion
}