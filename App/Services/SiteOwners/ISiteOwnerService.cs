using BlazorHomeSite.Data.SiteOwners;

namespace BlazorHomeSite.Services.SiteOwners;

public interface ISiteOwnerService
{
    public const int SiteOwnerAlbumId = 999999;

    SiteOwner GetSiteOwner();

    Task<SiteOwner> GetSiteOwnerAsync();

    void UpdateOrCreateSiteOwner(SiteOwner siteOwner);
}