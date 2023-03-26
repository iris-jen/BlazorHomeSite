using BlazorHomeSite.Data.Domain;
using BlazorHomeSite.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Services.SiteSettings;

public class SiteSettingsService : ISiteSettingsService
{
    private const int setSiteOwnerId = 1;
    private const int unsetSiteOwnerId = -99;

    private readonly IDatabaseService _databaseService;

    public SiteSettingsService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public SiteOwner GetSiteOwner()
    {
        return _databaseService.SiteOwners.FirstOrDefault() ??
               new SiteOwner() { Id = unsetSiteOwnerId };
    }

    public async Task<SiteOwner> GetSiteOwnerAsync()
    {
        return await _databaseService.SiteOwners.FirstOrDefaultAsync() ??
               new SiteOwner() { Id = unsetSiteOwnerId };
    }

    public void UpdateOrCreateSiteOwner(SiteOwner siteOwner)
    {
        if (siteOwner.Id == unsetSiteOwnerId)
        {
            siteOwner.Id = setSiteOwnerId;
            _databaseService.SiteOwners.Add(siteOwner);
        }
        else
        {
            _databaseService.SiteOwners.Update(siteOwner);
        }
    }
}