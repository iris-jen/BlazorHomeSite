namespace BlazorHomeSite.Data.Domain;

public class SiteOwner : BaseEntity
{
    public string? About { get; set; }
    public string? DiscordUrl { get; set; }
    public bool EnableBlogFeature { get; set; }
    public bool EnableMusicFeature { get; set; }
    public bool EnablePhotosFeature { get; set; }
    public string? FirstName { get; set; }
    public string? GitHubUrl { get; set; }
    public Photo? HomePageBackground { get; set; }
    public string? Introduction { get; set; }
    public string? LastName { get; set; }
    public string? LinkdinUrl { get; set; }
    public string? Location { get; set; }
    public Photo? ProfilePhoto { get; set; }
}