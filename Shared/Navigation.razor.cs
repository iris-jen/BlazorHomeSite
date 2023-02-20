using BlazorHomeSite.Data;
using BlazorHomeSite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazorHomeSite.Shared;

public partial class Navigation
{
    [Inject] private IDbContextFactory<HomeSiteDbContext>? DbFactory { get; set; }

    [Inject] private IOptions<AppAdminOptions>? options { get; set; }

    [CascadingParameter] private Task<AuthenticationState>? authenticationState { get; set; }

    private bool ShowAdminScreens { get; set; }

    private static string GetAlbumRoute(int id)
    {
        return $"/photoAlbum/{id}";
    }

    private string? GetThumbnailForAlbum(int id)
    {
        if (DbFactory != null)
        {
            using var context = DbFactory.CreateDbContext();
            var cover = context.Photos.FirstOrDefault(x => x.AlbumId == id && x.IsAlbumCover);
            return cover?.ThumbnailPath;
        }

        return string.Empty;
    }

    private List<PhotoAlbum> GetAllAlbums()
    {
        if (DbFactory == null) return new List<PhotoAlbum>();

        using var context = DbFactory.CreateDbContext();
        var albums = context.PhotoAlbums.Where(x => x.Description != "All The Rest").ToList();
        return albums;
    }

    protected override async Task OnInitializedAsync()
    {
        if (authenticationState is not null)
        {
            var email = "wrong@wrong.com";
            if (options != null)
            {
                email = options.Value.FromEmailAddress;
            }

            var authState = await authenticationState;
            var user = authState?.User;

            if (user?.Identity is not null && user.Identity.IsAuthenticated && user.IsInRole("Admin"))
            {
                ShowAdminScreens = true;
            }
        }
    }
}