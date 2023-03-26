using BlazorHomeSite.Data.Domain;
using FluentAssertions;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Data;

public class MusicAlbumModelTests
{
    [Fact]
    public void GivenMusic_AlbumWhenConstructedCorrectly_ThenValuesSet()
    {
        var name = "cool songs for cool folks";
        var description = "wow such muzac";
        var dateRecorded = DateTime.Now;

        var album = new MusicAlbum(name, description, dateRecorded);
        album.AlbumName.Should().Be(name);
        album.Description.Should().Be(description);
        album.DateRecorded.Should().Be(dateRecorded);
    }

    [Fact]
    public void GivenMusicAlbum_WhenAlbumCoverIdUpdated_ThenSets()
    {
        var album = new MusicAlbum("abc", "cba", DateTime.Now);
        album.UpdateAlbumCoverPhotoId(99);
        album.AlbumCoverPhotoId.Should().Be(99);
    }

    [Fact]
    public void GivenMusicAlbum_WhenConstructedWithNullStrings_ThenThrows()
        => Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new MusicAlbum(null, null, DateTime.Now);
        });

    [Fact]
    public void GivenMusicAlbum_WhenUpdatingDescription_ThenSets()
    {
        // Given
        var initialDescription = "best songs ever";
        var initialName = "jim bims big shiny tunes";
        var dateRecorded = new DateTime(1993, 10, 1);
        var album = new MusicAlbum(initialName, initialDescription, dateRecorded);

        album.Description.Should().Be(initialDescription);
        album.AlbumName.Should().Be(initialName);
        album.DateRecorded.Should().Be(dateRecorded);

        // When
        album.UpdateAlbumDescription("hot dog");
        album.Description.Should().Be("hot dog");
    }

    [Fact]
    public void GivenMusicAlbum_WhenUpdatingValuesWithBadShit_Throws()
    {
        var album = new MusicAlbum("abc", "cba", DateTime.Now);

        album.Invoking(x => x.UpdateAlbumDescription(null)).Should().Throw<ArgumentNullException>();
        album.Invoking(x => x.UpdateAlbumName(string.Empty)).Should().Throw<ArgumentException>();
        album.Invoking(x => x.UpdateDateRecorded(DateTime.MaxValue.AddDays(1))).Should().Throw<ArgumentException>();
    }
}