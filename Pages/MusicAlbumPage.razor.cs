using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Music;
using BlazorHomeSite.Shared;
using Howler.Blazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Pages;

public partial class MusicAlbumPage
{
    private static Task? playbackTimeTask;
    private static CancellationTokenSource? playbackTaskCancelSource = new();

    private Album? album;
    private TimeSpan currentPlayTime;
    private Song? selectedSong;
    private int soundId = -1;
    private TimeSpan totalSongTime;
    [Parameter] public string? AlbumId { get; set; }
    [Inject] private IDbContextFactory<HomeSiteDbContext>? DbFactory { get; set; }
    [Inject] private IHowl? Howl { get; set; }
    [Inject] private IHowlGlobal? HowlGlobal { get; set; }
    [Inject] private ILogger<MusicAlbumPage>? Logger { get; set; }
    [Inject] private NavigationManager? Nav { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await using var context = await DbFactory?.CreateDbContextAsync()!;

        var parsedId = -1;
        if (AlbumId != null) parsedId = int.Parse(AlbumId);

        album = await context.Albums.Include(x => x.Songs)
            .FirstOrDefaultAsync(x => x.Id == parsedId);

        if (album != null) selectedSong = album.Songs.FirstOrDefault();

        if (HowlGlobal != null && Howl != null)
        {
            Howl.OnPlay += e =>
            {
                totalSongTime = e.TotalTime;
                StateHasChanged();
            };

            Howl.OnStop += e => { StateHasChanged(); };
            Howl.OnPause += e => { StateHasChanged(); };
        }

        Nav.LocationChanged += LocationChanged;
        MainLayout.ScreenTitle = album.AlbumName;
        base.OnInitialized();
    }

    private void LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        Stop().ConfigureAwait(false);
    }

    protected async Task SetSelectedSong(Song songToPlay)
    {
        selectedSong = songToPlay;
        await Play();
    }

    protected async Task Play()
    {
        if (soundId != -1) await Stop();

        if (Howl != null && selectedSong != null)
        {
            soundId = await Howl.Play(selectedSong.Path);
            playbackTaskCancelSource = new CancellationTokenSource();
            playbackTimeTask = Task.Factory.StartNew(() => UpdatePlaybackTime(playbackTaskCancelSource.Token),
                playbackTaskCancelSource.Token);
        }
    }

    private async Task UpdatePlaybackTime(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
            try
            {
                currentPlayTime = await Howl.GetCurrentTime(soundId);
                await InvokeAsync(StateHasChanged);
                await Task.Delay(100, token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                await Task.Delay(1000);
                Logger.LogError(ex, "uh oh");
            }
    }

    protected async Task Stop()
    {
        playbackTaskCancelSource?.Cancel();
        if (Howl != null) await Howl.Stop(soundId);
    }

    protected async Task Pause()
    {
        if (Howl != null) await Howl.Pause(soundId);
    }
}