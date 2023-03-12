using BlazorHomeSite.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BlazorHomeSite.Data.Domain;

public class PhotoAlbum : BaseEntity, IAggregateRoot
{
    [Required]
    public string Description { get; private set; }

    [Required]
    public string Name { get; private set; }

    public List<Photo>? Photos { get; private set; }

    [Required]
    public UserLevel UserLevel { get; private set; }

    public PhotoAlbum(string name, string description, UserLevel userLevel)
    {
        Name = name;
        Description = description;
        UserLevel = userLevel;
    }

    public void UpdateDescription(string description)
    {
    }

    public void UpdateName(string name)
    {
    }

    public void UpdateUserLevel(string userLevel)
    {
    }
}