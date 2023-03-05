using Domain;

namespace Services.Owner
{
    public interface ISiteOwnerService
    {
        Task<SiteOwner> GetSiteOwnerAsync();

        Task UpdateOrCreateSiteOwner(SiteOwner siteOwner);
    }
}