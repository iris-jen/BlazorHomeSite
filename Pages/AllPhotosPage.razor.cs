using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Pages;

public partial class AllPhotosPage
{
    private const int photosPerPage = 50;
    protected Dictionary<int, int> yearPages = new();
    protected Dictionary<int, List<Photo>> yearPhotos = new();
    protected List<int> years = new();
    protected Dictionary<int, int> yearSelectedPages = new();
    [Inject] private IDbContextFactory<HomeSiteDbContext>? DbFactory { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await using var context = await DbFactory?.CreateDbContextAsync()!;
        years = await context.Photos.Select(x => x.CaptureTime.Year).Distinct().ToListAsync();

        if (years.Contains(1)) years.Remove(1);

        foreach (var year in years)
        {
            yearPhotos.Add(year, await context.Photos.Where(x => x.CaptureTime.Year == year)
                .OrderBy(x => x.CaptureTime).ToListAsync());

            yearPages.Add(year,
                (int)double.Round(context.Photos.Count(x => x.CaptureTime.Year == year) / photosPerPage));

            yearSelectedPages.Add(year, 1);
        }
    }

    protected void TriggerStateChange()
    {
        StateHasChanged();
    }
}