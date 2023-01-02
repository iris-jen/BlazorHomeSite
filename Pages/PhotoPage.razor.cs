using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Pages;

public partial class PhotoPage
{
    private readonly SortedList<int, Photo?> _lastPhotos = new();
    private readonly SortedList<int, Photo?> _nextPhotos = new();

    private SortedList<int, Photo?> _photos = new();
    [Inject] private IDbContextFactory<ApplicationDbContext>? DbFactory { get; set; }
    [Inject] private IWebHostEnvironment HostEnvironment { get; set; } = null!;

    [Parameter]
    public string? AlbumId { get; set; }

    [Parameter] public string? PhotoId { get; set; }

    protected int CurrentPhotoIndex;

    protected Photo? CurrentPhoto;

    private static System.Timers.Timer? autoPlayTimer;

    protected override async Task OnInitializedAsync()
    {
        await using var context = await DbFactory?.CreateDbContextAsync()!;

        var photoId = int.Parse(PhotoId ?? "-1");
        if (AlbumId != null && AlbumId.StartsWith("year_"))
        {
            var year = int.Parse(AlbumId.Split('_')[1]);
            _photos = await GetAllPhotos(photoId, year, true);
        }
        else if (AlbumId != null)
        {
            _photos = await GetAllPhotos(photoId, int.Parse(AlbumId));
        }
    }

    private void StartAutoPlay()
    {
        autoPlayTimer = new System.Timers.Timer(1000);

        autoPlayTimer.Elapsed += AutoPlayTimer_Elapsed;
        autoPlayTimer.AutoReset = true;
        autoPlayTimer.Enabled = true;
    }

    private void AutoPlayTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        NextPhoto();
    }

    private void StopAutoPlay()
    {
        if (autoPlayTimer != null)
        {
            autoPlayTimer.Stop();
            autoPlayTimer.Dispose();
        }
    }

    private async Task<SortedList<int, Photo?>> GetAllPhotos(int photoId, int albumId, bool getByYear = false)
    {
        List<Photo>? photos = null;
        if (DbFactory != null)
        {
            if (getByYear)
            {
                await using var context = await DbFactory.CreateDbContextAsync();
                photos = context.Photos.Where(x => x.CaptureTime.Year == albumId)
                    .OrderBy(x => x.CaptureTime).ToList();
            }
            else
            {
                photos = DataHelper.GetPhotoAlbum(DbFactory, albumId).Photos;
            }
        }

        var outputPhotos = new SortedList<int, Photo?>();
        if (photos != null)
        {
            var photoArray = photos.OrderBy(x => x.CaptureTime).ToArray();

            for (var i = 0; i < photoArray.Length; i++)
            {
                outputPhotos.Add(i, photoArray[i]);
                if (photoArray[i].Id != photoId) continue;
                CurrentPhoto = photoArray[i];
                CurrentPhotoIndex = i;
            }

            SetNextAndPreviousPhotos(CurrentPhotoIndex, outputPhotos);
        }

        return outputPhotos;
    }

    private void SetNextAndPreviousPhotos(int currentIndex, SortedList<int, Photo?> photos, int nextPhotoLimit = 3)
    {
        if (photos.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(photos));

        _nextPhotos.Clear();
        _lastPhotos.Clear();

        for (var i = currentIndex + 1;
             i < currentIndex + nextPhotoLimit + 1 && i <= photos.Count - 1;
             i++)
            _nextPhotos.Add(i, photos[i]);

        for (var i = currentIndex - 1;
             i > currentIndex - nextPhotoLimit - 1 && i >= 0;
             i--)
            _lastPhotos.Add(i, photos[i]);
    }

    private void NextPhoto()
    {
        if (CurrentPhotoIndex == _photos.Count - 1) return;
        CurrentPhotoIndex++;

        CurrentPhoto = _photos[CurrentPhotoIndex];

        SetNextAndPreviousPhotos(CurrentPhotoIndex, _photos);
        StateHasChanged();
    }

    private void LastPhoto()
    {
        if (CurrentPhotoIndex == 0) return;
        CurrentPhotoIndex--;
        CurrentPhoto = _photos[CurrentPhotoIndex];

        SetNextAndPreviousPhotos(CurrentPhotoIndex, _photos);
        StateHasChanged();
    }

    private async Task DeletePhoto()
    {
        await using var context = await DbFactory?.CreateDbContextAsync()!;

        if (CurrentPhoto != null)
        {
            var id = CurrentPhoto.Id;
            var thumbnailPath = CurrentPhoto.ThumbnailPath;
            var photoPath = CurrentPhoto.PhotoPath;

            var fullThumbPath = Path.Combine(HostEnvironment.WebRootPath, thumbnailPath ?? string.Empty);
            var fullPhotoPath = Path.Combine(HostEnvironment.WebRootPath, photoPath ?? string.Empty);
            NextPhoto();

            context.Photos.Remove(new Photo { Id = id });
            File.Delete(fullThumbPath);
            File.Delete(fullPhotoPath);
        }

        await context.SaveChangesAsync();
    }
}