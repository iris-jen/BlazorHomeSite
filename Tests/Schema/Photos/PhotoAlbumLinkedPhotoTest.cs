using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Tests.Schema.Photos
{
    public class PhotoAlbumLinkedPhotoTest : SqliteInMemoryDbTestBase
    {
        [Fact]
        public void Test()
        {
            using var _homeSiteDbContext = GetContext();

            var name = "coolest album evaaarrrr";
            var albumDescription = "Cool Album";
            var userLevel = UserLevel.Registered;
            var photoAlbum = new PhotoAlbum(name, albumDescription, userLevel);
            _homeSiteDbContext.PhotoAlbums.Add(photoAlbum);
            _homeSiteDbContext.SaveChanges();

            var photos = new List<Photo>()
            {
                new("a/b/c.webp", DateTime.Now, photoAlbum.Id),
                new("g/z/a.webp", DateTime.Now.AddDays(-1), photoAlbum.Id)
            };

            _homeSiteDbContext.Photos.AddRange(photos);
            _homeSiteDbContext.SaveChanges();

            var album = _homeSiteDbContext
                                .PhotoAlbums
                                .Include(x => x.Photos)
                                .AsSplitQuery()
                                .FirstOrDefault(x => x.Id == photoAlbum.Id);

            Assert.NotNull(album);
            album.Description.Should().Be(albumDescription);
            album.UserLevel.Should().Be(userLevel);

            album.Photos.Should().NotBeNull();
            album.Photos.Should().HaveCount(2);
            album.Photos.Should().BeEquivalentTo(photoAlbum.Photos);
        }
    }
}