using BlazorHomeSite.Data.Interfaces;

namespace BlazorHomeSite.Data.Domain;

public class PhotoAlbum : BaseEntity, IAggregateRoot
{
    public string? Description { get; set; }

    public IEnumerable<Photo> Photos => _photos.AsReadOnly();

    public IEnumerable<Tag> Tags => _tags.AsReadOnly();

    public string? UserLevel { get; set; }

    private readonly List<Photo> _photos = new();

    private readonly List<Tag> _tags = new();

    public PhotoAlbum(string description, string userLevel)
    {
    }
}