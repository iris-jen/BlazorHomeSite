using BlazorHomeSite.Data;
using BlazorHomeSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BlazorHomeSite.Pages
{
    public partial class Index
    {
        [Inject] private ISiteOwnerService SiteOwnerService { get; set; } = null!;

        private SiteOwner SiteOwner = null!;

        protected override async Task OnInitializedAsync()
        {
            SiteOwner = await SiteOwnerService.GetSiteOwnerAsync();
        }
    }
}