using BlazorHomeSite.Data;
using BlazorHomeSite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Services
{
    public class SiteOwnerService : ISiteOwnerService
    {
        private readonly IDbContextFactory<HomeSiteDbContext> _dbFactory;

        public SiteOwnerService(IDbContextFactory<HomeSiteDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<SiteOwner> GetSiteOwnerAsync()
        {
            var context = await _dbFactory.CreateDbContextAsync();
            return await context.SiteOwners.FirstOrDefaultAsync() ?? new SiteOwner();
        }

        public async Task UpdateOrCreateSiteOwner(SiteOwner siteOwner)
        {
            var context = await _dbFactory.CreateDbContextAsync();
            if (await context.SiteOwners.CountAsync() == 0)
            {
                await context.SiteOwners.AddAsync(siteOwner);
            }
            else
            {
                context.SiteOwners.Update(siteOwner);
            }

            await context.SaveChangesAsync();
        }
    }
}