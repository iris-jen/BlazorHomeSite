using BlazorHomeSite.Data.SiteOwners;
using BlazorHomeSite.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Services.SiteOwners;

public class SiteOwnerService : ISiteOwnerService
{
    public const int SetSiteOwnerId = 1;
    public const int UnsetSiteOwnerId = -99;

    private readonly IDatabaseService _databaseService;

    public SiteOwnerService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public SiteOwner GetSiteOwner()
    {
        return _databaseService.SiteOwners.FirstOrDefault() ??
               new SiteOwner() { Id = UnsetSiteOwnerId };
    }

    public async Task<SiteOwner> GetSiteOwnerAsync()
    {
        return await _databaseService.SiteOwners.FirstOrDefaultAsync() ??
               new SiteOwner() { Id = UnsetSiteOwnerId };
    }

    public void UpdateOrCreateSiteOwner(SiteOwner siteOwner)
    {
        if (siteOwner.Id == UnsetSiteOwnerId)
        {
            siteOwner.Id = SetSiteOwnerId;
            _databaseService.SiteOwners.Add(siteOwner);
        }
        else
        {
            _databaseService.SiteOwners.Update(siteOwner);
        }
        _databaseService.SaveDb();
    }
}