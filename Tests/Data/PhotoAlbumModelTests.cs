using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Domain;
using FluentAssertions;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace Tests.Data;

public class PhotoAlbumModelTests
{
    [Fact]
    public void GivenPhotoAlbum_WhenConstructed_Correctly_ThenPropertiesSet()
    {
        var name = "hotdog";
        var description = "hotdogs";
        var userLevel = UserLevel.Owner;
        var photoAlbum = new PhotoAlbum(name, description, userLevel);

        photoAlbum.Name.Should().Be(name);
        photoAlbum.Description.Should().Be(description);
        photoAlbum.UserLevel.Should().Be(userLevel);
    }

    [Fact]
    public void GivenPhotoAlbum_WhenUserLevelUpdated_ThenSet()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Max);
        album.UpdateUserLevel(UserLevel.NoAccount);
        album.UserLevel.Should().Be(UserLevel.NoAccount);
    }

    [Fact]
    public void GivenPhotoAlbum_WhenUpdatingOrderWithNegative_ThenThrows()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Max);
        album.Invoking(x => x.UpdateAlbumOrder(-9)).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenPhotoAlbum_WhenUpdatingOrder_ThenSet()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Max);
        album.UpdateAlbumOrder(9);
        album.AlbumOrder.Should().Be(9);
    }

    [Fact]
    public void GivenPhotoAlbum_WhenDescriptionUpdatedWithEmpty_ThenThrows()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Max);
        album.Invoking(x => x.UpdateDescription(string.Empty)).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenPhotoAlbum_WhenDescriptionUpdated_ThenDescriptionUpdated()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Max);
        album.UpdateDescription("abc");
        album.Description.Should().Be("abc");
    }

    [Fact]
    public void GivenPhotoAlbum_WhenNameUpdatedWithNull_ThenThrows()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Max);
        album.Invoking(x => x.UpdateName(null)).Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenPhotoAlbum_WhenNameUpdated_ThenUpdated()
    {
        var album = new PhotoAlbum("a", "b", UserLevel.Max);
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