using BlazorHomeSite.Data;

namespace BlazorHomeSite.Services.Interfaces
{
    public interface ISiteOwnerService
    {
        Task<SiteOwner> GetSiteOwnerAsync();

        Task UpdateOrCreateSiteOwner(SiteOwner siteOwner);
    }
}