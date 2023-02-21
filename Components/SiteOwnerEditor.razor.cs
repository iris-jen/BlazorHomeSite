using BlazorHomeSite.Data;
using BlazorHomeSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace BlazorHomeSite.Components;

public partial class SiteOwnerEditor
{
    [Inject] private ISiteOwnerService SiteOwnerService { get; set; } = null!;

    [Inject] private ILogger<SiteOwnerEditor> Logger { get; set; } = null!;

    private SiteOwner OwnerModel = new SiteOwner();

    private bool Read = true;

    protected override async Task OnInitializedAsync()
    {
        OwnerModel = await SiteOwnerService.GetSiteOwnerAsync();
    }

    private async Task ToggleEdit()
    {
        if (Read)
        {
            Read = false;
        }
        else
        {
            await CancelEdit();
        }
    }

    private async Task UpdateSiteOwner()
    {
        await SiteOwnerService.UpdateOrCreateSiteOwner(OwnerModel);
        Read = true;
    }

    private async Task CancelEdit()
    {
        OwnerModel = await SiteOwnerService.GetSiteOwnerAsync();
        Read = true;
    }
}