using Ardalis.GuardClauses;
using BlazorHomeSite.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlazorHomeSite.Data.Photos;

public class PhotoAlbum : BaseEntity
{
    public int AlbumOrder { get; private set; }

    [Required]
    public string Description { get; private set; }

    [Required]
    public string Name { get; private set; }

    public List<Photo>? Photos { get; private set; }

    [Required]
    public UserLevel UserLevel { get; private set; }

    public PhotoAlbum(string name, string description, UserLevel userLevel)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Description = Guard.Against.NullOrEmpty(description);
        UserLevel = userLevel;
    }

    public void UpdateAlbumOrder(int order)
        => AlbumOrder = Guard.Against.Negative(order);

    public void UpdateDescription(string description)
        => Description = Guard.Against.NullOrEmpty(description);

    public void UpdateName(string name)
        => Name = Guard.Against.NullOrEmpty(name);

    public void UpdateUserLevel(UserLevel userLevel)
        => UserLevel = userLevel;
}