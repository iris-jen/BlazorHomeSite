using BlazorHomeSite.Data.Photos;
using BlazorHomeSite.Services.Database;
using BlazorHomeSite.Testing;
using FluentAssertions;
using LazyCache;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BlazorHomeSite.Services.Photos.Photos
{
    public class PhotosServicePagingTests : SqliteInMemoryDbTestBase
    {
        private const int _photoAlbumId = 44;
        private const int _existingPhotos = 20;
        private readonly IDatabaseService _databaseService;
        private readonly AppAdminOptions _appAdminOptions;
        private readonly IPhotosService _photosService;

        public PhotosServicePagingTests()
        {
            _databaseService = GetContext();

            #region Setup App Options Mock

            _appAdminOptions = new AppAdminOptions()
            {
                PhotoDirectory = "Test"
            };
            var optionsMock = new Mock<IOptions<AppAdminOptions>>();
            optionsMock.Setup(x => x.Value).Returns(_appAdminOptions);

            #endregion Setup App Options Mock

            #region Setup Photo Service

            var cacheMock = new Mock<IAppCache>();

            var photoServiceMock = new Mock<PhotosService>(cacheMock.Object, _databaseService);

            photoServiceMock.Setup(x => x.GetImageB64(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                                         .ReturnsAsync("..");

            photoServiceMock.Setup(x => x.ReadB64FromFile(It.IsAny<bool>(), It.IsAny<string>()))
                                         .ReturnsAsync("..");

            _photosService = photoServiceMock.Object;

            #endregion Setup Photo Service

            #region Pre-populate DB with photo's

            var photoAlbum = new PhotoAlbum("Test", "testing", Data.Enums.UserLevel.NoAccount);
            photoAlbum.Id = _photoAlbumId;
            _databaseService.PhotoAlbums.Add(photoAlbum);
            _databaseService.SaveDb();

            for (int i = 0; i < _existingPhotos; i++)
            {
                var photo = new Photo("test", DateTime.Now, photoAlbum.Id);
                photo.UpdateDescription($"{i + 1}");
                _databaseService.Photos.Add(photo);
            }

            _databaseService.SaveDb();

            #endregion Pre-populate DB with photo's
        }

        [Fact]
        public async Task GivenServiceWith20Items_WhenGetImageByPage_WithPage1Items5_ThenReturnsFirst5()
        {
            var page = 1;
            var images = 5;

            var returnedImages = await _photosService.GetB64ImagesByPage(_photoAlbumId, page, images);

            returnedImages.Count.Should().Be(images);
            returnedImages.Should().ContainKeys(1, 2, 3, 4, 5);
        }

        [Fact]
        public async Task GivenServiceWith20Items_WhenGetImageByPage_WithPage2Items5_ThenReturnsNext5()
        {
            var page = 2;
            var images = 5;

            var returnedImages = await _photosService.GetB64ImagesByPage(_photoAlbumId, page, images);
            returnedImages.Count.Should().Be(images);
            returnedImages.Should().ContainKeys(6, 7, 8, 9, 10);
        }

        [Fact]
        public async Task GivenServiceWith20Items_WhenGetImageByPage_WithPage1Items20_ThenReturns20()
        {
            var page = 1;
            var images = 20;

            var returnedImages = await _photosService.GetB64ImagesByPage(_photoAlbumId, page, images);
            returnedImages.Count.Should().Be(20);
            returnedImages.Should().ContainKeys(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        }

        [Fact]
        public async Task GivenServiceWith20Items_WhenGetImageByPage_WithPage1Items40_ThenReturns20()
        {
            var returnedImages = await _photosService.GetB64ImagesByPage(_photoAlbumId, 1, 40);
            returnedImages.Count.Should().Be(20);
            returnedImages.Should().ContainKeys(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        }
    }
}