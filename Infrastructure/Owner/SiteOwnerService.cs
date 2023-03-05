using Ardalis.Specification;
using Domain;

namespace Services.Owner
{
    public class SiteOwnerService : ISiteOwnerService
    {
        private readonly IRepositoryBase<SiteOwner> _siteOwnerRepo;

        public SiteOwnerService(IRepositoryBase<SiteOwner> siteOwnerRepo)
        {
            _siteOwnerRepo = siteOwnerRepo;
        }

        public async Task<SiteOwner> GetSiteOwnerAsync()
        {
            var ownerList = await _siteOwnerRepo.ListAsync();
            return ownerList.Any() ? ownerList[0] : new SiteOwner();
        }

        public async Task UpdateOrCreateSiteOwner(SiteOwner siteOwner)
        {
            if (await _siteOwnerRepo.CountAsync() == 0)
            {
                await _siteOwnerRepo.AddAsync(siteOwner);
            }
            else
            {
                await _siteOwnerRepo.UpdateAsync(siteOwner);
            }

            await _siteOwnerRepo.SaveChangesAsync();
        }
    }
}