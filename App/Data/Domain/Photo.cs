using Ardalis.GuardClauses;
using BlazorHomeSite.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BlazorHomeSite.Data.Domain;

public class Photo : BaseEntity, IAggregateRoot
{
    #region Properties

    public PhotoAlbum? Album { get; private set; }

    [Required]
    public int AlbumId { get; private set; }

    [Required]
    public DateTime CaptureTime { get; private set; }

    public string? Description { get; private set; }
    public bool IsAlbumCover { get; private set; }
    public string? Location { get; private set; }
    public string? LocationCoOrdinates { get; private set; }

    public List<Comment>? PhotoComments { get; private set; }

    [Required]
    public string PhotoPath { get; private set; }

    public List<Tag>? PhotoTags { get; private set; }
    public int Rotation { get; private set; }

    [Required]
    public string ThumbnailPath { get; private set; }

    #endregion Properties

    public Photo(string photoPath, string thumbnailPath, DateTime captureTime, int albumId)
    {
        PhotoPath = Guard.Against.NullOrEmpty(photoPath);
        ThumbnailPath = Guard.Against.NullOrEmpty(thumbnailPath);
        CaptureTime = Guard.Against.OutOfSQLDateRange(captureTime);
        AlbumId = Guard.Against.Negative(albumId);
    }

    #region Methods

    public void UpdateDescription(string description)
        => Description = Guard.Against.NullOrEmpty(description);

    public void UpdateIsAlbumCover(bool isAlbumCover)
        => IsAlbumCover = isAlbumCover;

    public void UpdateLocation(string location)
        => Location = Guard.Against.NullOrEmpty(location);

    public void UpdateLocationCoOrdinates(string coOrdinates)
        => LocationCoOrdinates = Guard.Against.NullOrEmpty(coOrdinates);

    public void UpdateRotation(int rotation)
        => Rotation = Guard.Against.OutOfRange(rotation, nameof(rotation), 0, 360);

    #endregion Methods
}