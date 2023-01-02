using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Music;
using Howler.Blazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Pages;

public partial class MusicAlbumPage
{
    [Parameter] public string? AlbumId { get; set; }
    [Inject] private IDbContextFactory<ApplicationDbContext>? DbFactory { get; set; }
    [Inject] private IHowl? Howl { get; set; }
    [Inject] private IHowlGlobal? HowlGlobal { get; set; }

    protected Album? album;
    protected TimeSpan totalSongTime;
    protected string playbackState = "-";
    protected string? supportedCodes;
    protected Song? selectedSong;
    protected int soundId = -1;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await using var context = await DbFactory?.CreateDbContextAsync()!;

        var parsedId = -1;
        if (AlbumId != null) parsedId = int.Parse(AlbumId);

        album = await context.Albums.Include(x => x.Songs)
                                    .FirstOrDefaultAsync(x => x.Id == parsedId);

        if (album != null)
        {
            selectedSong = album.Songs.FirstOrDefault();
        }

        if (HowlGlobal != null && Howl != null)
        {
            var codecs = await HowlGlobal.GetCodecs();
            supportedCodes = string.Join(", ", codecs);

            Howl.OnPlay += e =>
            {
                playbackState = "Playing";
                totalSongTime = e.TotalTime;

                StateHasChanged();
            };

            Howl.OnStop += e =>
            {
                playbackState = "Stopped";
                StateHasChanged();
            };

            Howl.OnPause += e =>
            {
                playbackState = "Paused";
                StateHasChanged();
            };
        }
    }

    protected async Task SetSelectedSong(Song songToPlay)
    {
        selectedSong = songToPlay;
        if (soundId != -1)
        {
            await Stop();
        }
        await Play();
    }

    protected async Task Play()
    {
        if (Howl != null && selectedSong != null)
        {
            soundId = await Howl.Play(selectedSong.Path);
        }
    }

    protected async Task Stop()
    {
        if (Howl != null)
        {
            await Howl.Stop(soundId);
        }
    }

    protected async Task Pause()
    {
        if (Howl != null)
        {
            await Howl.Pause(soundId);
        }
    }
}