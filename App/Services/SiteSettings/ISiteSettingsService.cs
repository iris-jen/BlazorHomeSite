using BlazorHomeSite.Data.Domain;

namespace BlazorHomeSite.Services.SiteSettings;

public interface ISiteSettingsService
{
    Task<SiteOwner> GetSiteOwnerAsync();

    Task UpdateOrCreateSiteOwner(SiteOwner siteOwner);
}