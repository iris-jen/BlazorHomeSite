using BlazorHomeSite.Data.Enums;
using FluentAssertions;
using Xunit;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace BlazorHomeSite.Data.Photos;

public class PhotoAlbumTests
{
    [Fact]
    public void GivenPhotoAlbum_WhenConstructed_Correctly_ThenPropertiesSet()
    {
        var name = "hotdog";
        var description = "hotdogs";
        var userLevel = UserLevel.Admin;
        var photoAlbum = new PhotoAlbum(name, description, userLevel);

        photoAlbum.Name.Should().Be(name);
        photoAlbum.Description.Should().Be(description);
        photoAlbum.UserLevel.Should().Be(userLevel);
    }

    [Fact]
    public void GivenPhotoAlbum_WhenUserLevelUpdated_ThenSet()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Registered);
        album.UpdateUserLevel(UserLevel.NoAccount);
        album.UserLevel.Should().Be(UserLevel.NoAccount);
    }

    [Fact]
    public void GivenPhotoAlbum_WhenUpdatingOrderWithNegative_ThenThrows()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Registered);
        album.Invoking(x => x.UpdateAlbumOrder(-9)).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenPhotoAlbum_WhenUpdatingOrder_ThenSet()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Registered);
        album.UpdateAlbumOrder(9);
        album.AlbumOrder.Should().Be(9);
    }

    [Fact]
    public void GivenPhotoAlbum_WhenDescriptionUpdatedWithEmpty_ThenThrows()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Registered);
        album.Invoking(x => x.UpdateDescription(string.Empty)).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenPhotoAlbum_WhenDescriptionUpdated_ThenDescriptionUpdated()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Registered);
        album.UpdateDescription("abc");
        album.Description.Should().Be("abc");
    }

    [Fact]
    public void GivenPhotoAlbum_WhenNameUpdatedWithNull_ThenThrows()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Registered);
        album.Invoking(x => x.UpdateName(null)).Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenPhotoAlbum_WhenNameUpdated_ThenUpdated()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Registered);
        album.UpdateName("123abc");
        album.Name.Should().Be("123abc");
    }

    [Fact]
    public void GivenPhotoAlbum_WhenConstructed_WithNullStrings_ThenThrowsArgumentNull()
        => Assert.Throws<ArgumentNullException>(() =>
        {
            new PhotoAlbum(null, null, UserLevel.NoAccount);
        });
}

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.