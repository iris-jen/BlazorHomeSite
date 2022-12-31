using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Pages;

public partial class AllPhotosPage
{
    private const int PhotosPerPage = 50;
    [Inject] private IDbContextFactory<ApplicationDbContext>? DbFactory { get; set; }


    public int Page { get; set; }

    public List<Photo> PagePhotos { get; set; }

    public List<int> GetYears()
    {
        Page = 1;
        using var context = DbFactory?.CreateDbContext()!;
        return context.Photos.Select(x => x.CaptureTime.Year).Distinct().ToList();
    }

    private int GetPagesForYear(int year)
    {
        using var context = DbFactory?.CreateDbContext()!;
        var num = context.Photos.Count(x => x.CaptureTime.Year == year);

        return (int)double.Round(num / PhotosPerPage, 0);
    }

    public List<Photo> GetPhotosByYear(int year)
    {
        using var context = DbFactory?.CreateDbContext()!;

        PagePhotos = context.Photos.Where(x => x.CaptureTime.Year == year).OrderBy(x => x.CaptureTime)
            .ToList();

        return PagePhotos;
    }
}