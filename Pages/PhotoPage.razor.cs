using System.Timers;
using BlazorHomeSite.Data;
using BlazorHomeSite.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Timer = System.Timers.Timer;

namespace BlazorHomeSite.Pages;

public partial class PhotoPage
{
    private static Timer? _autoPlayTimer;
    private readonly SortedList<int, Photo?> _lastPhotos = new();
    private readonly SortedList<int, Photo?> _nextPhotos = new();
    private SortedList<int, Photo?> _photos = new();
    private List<Photo> _photoWindow = new();
    private Photo? _currentPhoto;

    private int _currentPhotoIndex;
    private int _carosuelIndex;
    private const int _carosuelMax = 8;
    public int ImageHeight { get; set; } = 500;
    public string CarosuelHeightTemplate => $"Height:{ImageHeight+20}px";

    private bool _slideShowOn;
    [Inject] private IDbContextFactory<ApplicationDbContext>? DbFactory { get; set; }
    [Inject] private IWebHostEnvironment HostEnvironment { get; set; } = null!;

    [Parameter] public string? AlbumId { get; set; }

    [Parameter] public string? PhotoId { get; set; }
    public double SlideShowTimeSeconds { get; set; } = 1.5;

    protected override async Task OnInitializedAsync()
    {
        await using var context = await DbFactory?.CreateDbContextAsync()!;

        var photoId = int.Parse(PhotoId ?? "-1");
        if (AlbumId != null && AlbumId.StartsWith("year_"))
        {
            var year = int.Parse(AlbumId.Split('_')[1]);
            _photos = await GetAllPhotos(photoId, year, true);
            MainLayout.ScreenTitle = $"Photos - {year}";
        }
        else if (AlbumId != null)
        {
            var albumId = int.Parse(AlbumId);
            _photos = await GetAllPhotos(photoId, albumId);
            var album = await context.PhotoAlbums.FirstOrDefaultAsync(x => x.Id == albumId);
            MainLayout.ScreenTitle = album != null ? album.Description ?? "" : "🐛🐛🐛🐛🐛🐛🐛🐛";
        }
    }

    private void StartAutoPlay()
    {
        _slideShowOn = true;
        _autoPlayTimer = new Timer(TimeSpan.FromSeconds(SlideShowTimeSeconds == 0 ? 0.1 : SlideShowTimeSeconds));

        _autoPlayTimer.Elapsed += AutoPlayTimer_Elapsed;
        _autoPlayTimer.AutoReset = true;
        _autoPlayTimer.Enabled = true;
    }

    private void AutoPlayTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        NextPhoto();
    }

    private void StopAutoPlay()
    {
        if (_autoPlayTimer != null)
        {
            _autoPlayTimer.Stop();
            _autoPlayTimer.Dispose();
        }

        _slideShowOn = false;
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
                _currentPhoto = photoArray[i];
                _currentPhotoIndex = i;
            }

            SetNextAndPreviousPhotos(_currentPhotoIndex, outputPhotos);
            _photoWindow.AddRange(outputPhotos.Values.Skip(_currentPhotoIndex).Take(8));
        }

        return outputPhotos;
    }

    private void SetNextAndPreviousPhotos(int currentIndex, SortedList<int, Photo?> photos, int nextPhotoLimit = 8)
    {
        if (photos.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(photos));

        _nextPhotos.Clear();
        _lastPhotos.Clear();

        var halfLimit = nextPhotoLimit / 2;
        for (var i = currentIndex + 1;
             i < currentIndex + halfLimit + 1 && i <= photos.Count - 1;
             i++)
            _nextPhotos.Add(i, photos[i]);

        for (var i = currentIndex - 1;
             i > currentIndex - halfLimit - 1 && i >= 0;
             i--)
            _lastPhotos.Add(i, photos[i]);
    }

    private void NextPhoto()
    {
        if (_currentPhotoIndex == _photos.Count - 1) return;
        _currentPhotoIndex++;

        if (_carosuelIndex < _carosuelMax-1)
        {
            _carosuelIndex++;
        }
        else
        {
            _photoWindow.Clear();
            _photoWindow.AddRange(_photos.Values.Skip(_currentPhotoIndex).Take(_carosuelMax));
            _carosuelIndex = 0;
        }

        _currentPhoto = _photos[_currentPhotoIndex];

        SetNextAndPreviousPhotos(_currentPhotoIndex, _photos);
        InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
    }

    private void LastPhoto()
    {
        if (_currentPhotoIndex == 0) return;
        _currentPhotoIndex--;
        _currentPhoto = _photos[_currentPhotoIndex];

        if (_carosuelIndex > 0)
        {
            _carosuelIndex--;
        }
        else
        {
            var skip = _currentPhotoIndex - _carosuelMax > 0 ? _currentPhotoIndex - _carosuelMax : 0;
            _photoWindow.Clear();
            _photoWindow.AddRange(_photos.Values.Skip(skip).Take(_carosuelMax));
            _carosuelIndex = _carosuelMax-1;
        }

        SetNextAndPreviousPhotos(_currentPhotoIndex, _photos);
        InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
    }

    private async Task DeletePhoto()
    {
        await using var context = await DbFactory?.CreateDbContextAsync()!;

        if (_currentPhoto != null)
        {
            var id = _currentPhoto.Id;
            var thumbnailPath = _currentPhoto.ThumbnailPath;
            var photoPath = _currentPhoto.PhotoPath;

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