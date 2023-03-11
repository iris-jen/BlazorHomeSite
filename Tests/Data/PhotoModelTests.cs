﻿using BlazorHomeSite.Data.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR.Protocol;
using Xunit;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace Tests.Data;

public class PhotoModelTests
{
    [Fact]
    public void GivenPhoto_WhenConstructedWithNullStrings_ThenThrowsArgumentNull()
        => Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new Photo(null, "", DateTime.Now, 1);
        });

    [Fact]
    public void GivenPhoto_WhenConstructedWithOutOfRangeDateTime_ThenThrowsArgumentOutOfRange()
        => Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new Photo("a/b/c", "c/b/z", DateTime.MaxValue, 1);
        });

    [Fact]
    public void GivenPhoto_WhenConstructedWithNegativeAlbumId_ThenThrowsArgumentOutOfRange()
        => Assert.Throws<ArgumentException>(() =>
        {
            _ = new Photo("a/b/c", "c/b/z", DateTime.Now, -9000000);
        });

    [Fact]
    public void GivenPhoto_WhenConstructedCorectly_ThenPropertiesSet()
    {
        var photoPath = "test/path.jpg";
        var thumbnailPath = "test/thumbails/path.jpg";
        var time = DateTime.Now;
        var albumId = 20;

        var photo = new Photo(photoPath, thumbnailPath, time, albumId);
        photo.PhotoPath.Should().Be(photoPath);
        photo.ThumbnailPath.Should().Be(thumbnailPath);
        photo.CaptureTime.Should().Be(time);
        photo.AlbumId.Should().Be(albumId);
    }

    [Fact]
    public void GivenPhoto_WhenDescriptionUpdated_ThenDescriptionSet()
    {
        var photo = new Photo("ab", "ca", DateTime.Now, 1);
        photo.UpdateDescription("cool photo");
        photo.Description.Should().Be("cool photo");
    }

    [Fact]
    public void GivenPhoto_WhenDescriptionUpdatedWithNull_ThenThrows()
    {
        var photo = new Photo("ab", "ca", DateTime.Now, 1);
        photo.Invoking(photo => photo.UpdateDescription(null)).Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void GivenPhoto_WhenIsAlbumCoverUpdated_ThenAlbumCoverSet()
    {
        var photo = new Photo("ab", "ca", DateTime.Now, 1);
        photo.Invoking(x => x.UpdateIsAlbumCover(true)).Should().NotThrow();
        photo.IsAlbumCover.Should().BeTrue();
    }

    [Fact]
    public void GivenPhoto_WhenLocationUpdated_ThenLocationSet()
    {
        var photo = new Photo("ab", "ca", DateTime.Now, 1);
        photo.UpdateLocation("cool place");
        photo.Location.Should().Be("cool place");
    }

    [Fact]
    public void GivenPhoto_WhenLocationUpdatedIsEmpty_ThenThrows()
    {
        var photo = new Photo("ab", "ca", DateTime.Now, 1);
        photo.Invoking(x => x.UpdateLocation(string.Empty)).Should().Throw<ArgumentException>();
    }
}

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.