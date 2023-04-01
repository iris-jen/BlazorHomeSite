using BlazorHomeSite.Data.Domain;
using BlazorHomeSite.Services.SiteSettings;
using FluentAssertions;

namespace Tests.Services.SiteSettings
{
    public class UpdateOrCreateSiteOwnerTests : SqliteInMemoryDbTestBase
    {
        [Fact]
        public void ValidNewModel_CreatesNewDbEntry()
        {
            var siteOwnerService = new SiteOwnerService(GetContext());

            var siteOwner = new SiteOwner()
            {
                Id = SiteOwnerService.UnsetSiteOwnerId,
                About = "hello, abc 123",
                DiscordUrl = "a/a/a/a",
                Introduction = "yada yada yada",
                FirstName = "bobo",
                LastName = "dingdong",
            };

            siteOwnerService.UpdateOrCreateSiteOwner(siteOwner);

            var context = GetContext();
            context.SiteOwners.Should().HaveCount(1);

            var read = context.SiteOwners.FirstOrDefault();
            read.Id.Should().Be(SiteOwnerService.SetSiteOwnerId);
            read.Should().NotBeNull();
            read.Introduction.Should().Be(siteOwner.Introduction);
            read.About.Should().Be(siteOwner.About);
            read.DiscordUrl.Should().Be(siteOwner.DiscordUrl);
            read.FirstName.Should().Be(siteOwner.FirstName);
            read.LastName.Should().Be(siteOwner.LastName);
        }
    }
}