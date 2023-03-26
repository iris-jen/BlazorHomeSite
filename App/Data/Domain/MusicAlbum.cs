using Ardalis.GuardClauses;
using Newtonsoft.Json.Bson;
using System.ComponentModel.DataAnnotations;

namespace BlazorHomeSite.Data.Domain;

public class MusicAlbum : BaseEntity
{
    public int AlbumCoverPhotoId { get; private set; }

    [Required]
    [StringLength(80)]
    public string AlbumName { get; private set; }

    public DateTime DateRecorded { get; private set; }

    [Required]
    [StringLength(512)]
    public string Description { get; private set; }

    public List<Song>? Songs { get; private set; }

    public MusicAlbum(string albumName, string description, DateTime dateRecorded)
    {
        AlbumName = Guard.Against.NullOrEmpty(albumName);
        Description = Guard.Against.NullOrEmpty(description);
        DateRecorded = Guard.Against.OutOfSQLDateRange(dateRecorded);
    }

    public void UpdateAlbumCoverPhotoId(int id)
        => AlbumCoverPhotoId = id;

    public void UpdateAlbumDescription(string newDescription)
        => Description = Guard.Against.NullOrEmpty(newDescription);

    public void UpdateAlbumName(string newName)
        => AlbumName = Guard.Against.NullOrEmpty(newName);

    public void UpdateDateRecorded(DateTime dateRecorded)
        => DateRecorded = Guard.Against.OutOfSQLDateRange(dateRecorded);
}