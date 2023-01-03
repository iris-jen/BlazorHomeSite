using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Music;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Pages;

public partial class MusicAlbumsPage
{
    [Inject] private IDbContextFactory<ApplicationDbContext>? DbFactory { get; set; }

    public List<Album>? Albums { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        await using var context = await DbFactory?.CreateDbContextAsync()!;

        Albums = context.Albums.OrderBy(x => x.DateRecorded).ToList();
    }
}