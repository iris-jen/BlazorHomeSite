using Domain;
using Microsoft.AspNetCore.Components;
using Services.Owner;

namespace PresentationBlazor.Pages
{
    public partial class Index
    {
        [Inject] private ISiteOwnerService SiteOwnerService { get; set; } = null!;

        private SiteOwner SiteOwner { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            SiteOwner = await SiteOwnerService.GetSiteOwnerAsync();
        }
    }
}