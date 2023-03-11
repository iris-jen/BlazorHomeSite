using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Domain;
using BlazorHomeSite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazorHomeSite.Shared;

public partial class Navigation
{
    [Inject] private IDbContextFactory<HomeSiteDbContext>? DbFactory { get; set; }
    [Inject] private IOptions<AppAdminOptions>? Options { get; set; }
    [Inject] private RoleManager<IdentityRole> RoleManager { get; set; } = null!;
    [Inject] private SignInManager<IdentityUser> SignInManager { get; set; } = null!;
    [Inject] private UserManager<IdentityUser> UserManager { get; set; } = null!;

    private static string GetAlbumRoute(int id)
    {
        return $"/photoAlbum/{id}";
    }

    private List<PhotoAlbum> GetAllAlbums()
    {
        if (DbFactory == null) return new List<PhotoAlbum>();

        using var context = DbFactory.CreateDbContext();
        var albums = context.PhotoAlbums.Where(x => x.Description != "All The Rest").ToList();
        return albums;
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

    private async Task InitAdmin()
    {
        const string adminRoleName = "Admin";
        var users = await UserManager.GetUsersInRoleAsync(adminRoleName);

        if (users.Count == 0)
        {
            if (Options != null)
            {
                await RoleManager.CreateAsync(new IdentityRole(adminRoleName));
                var adminEmail = Options.Value.AdminEmailAddress;
                if (adminEmail != null)
                {
                    var userWithAdmin = await UserManager.FindByEmailAsync(adminEmail);
                    if (userWithAdmin != null)
                    {
                        await UserManager.AddToRoleAsync(userWithAdmin, "Admin");
                    }
                }
            }
        }
    }
}