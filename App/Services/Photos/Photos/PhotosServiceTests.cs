using BlazorHomeSite.Data.Photos;
using BlazorHomeSite.Services.Database;
using BlazorHomeSite.Services.Photos.PhotoAlbums;
using BlazorHomeSite.Testing;
using LazyCache;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BlazorHomeSite.Services.Photos.Photos
{
    public class PhotosServiceTests : SqliteInMemoryDbTestBase
    {
        private readonly IPhotoAlbumService _photoAlbumService;
        private readonly IDatabaseService _databaseService;
        private readonly AppAdminOptions _appAdminOptions;
        private readonly IPhotosService _photosService;

        public PhotosServiceTests()
        {
            _appAdminOptions = new AppAdminOptions()
            {
                PhotoDirectory = Directory.GetCurrentDirectory()
            };

            var optionsMock = new Mock<IOptions<AppAdminOptions>>();

            optionsMock.Setup(x => x.Value == _appAdminOptions);

            _databaseService = GetContext();
            _photoAlbumService = new PhotoAlbumService
                                    (_databaseService,
                                    optionsMock.Object);

            var cacheMock = new Mock<IAppCache>();
            _photosService = new PhotosService(cacheMock.Object, _databaseService);
        }

        [Fact]
        public void TestPagingWhenPopulated()
        {
            // Setup
            var testFile = File.Create("test");
            var photoAlbum = new PhotoAlbum("Test", "testing", Data.Enums.UserLevel.NoAccount);
            photoAlbum.Id = 44;
            _databaseService.PhotoAlbums.Add(photoAlbum);
            _databaseService.SaveDb();

            for (int i = 0; i < 20; i++)
            {
                var photo = new Photo("test", DateTime.Now, photoAlbum.Id);
                photo.UpdateDescription($"{i + 1}");
                _databaseService.Photos.Add(photo);
            }

            _databaseService.SaveDb();

            // Act
            _photosService.GetB64ImagesByPage(photoAlbum.Id, 1, 5);
        }
    }
}