using BlazorHomeSite.Data.Domain;
using BlazorHomeSite.Services.SiteSettings;
using Microsoft.AspNetCore.Components;

namespace BlazorHomeSite.Pages;

public partial class Index
{
    private SiteOwner SiteOwner { get; set; } = null!;
    [Inject] private ISiteSettingsService SiteOwnerService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        SiteOwner = await SiteOwnerService.GetSiteOwnerAsync();
    }
}