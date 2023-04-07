namespace BlazorHomeSite.Data.SiteOwners;

public class SiteOwner : BaseEntity
{
    public string? About { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Location { get; set; }

    public string? DiscordUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? Introduction { get; set; }
    public string? LinkdinUrl { get; set; }
    public int PhotoIdHomePageBackground { get; set; }
    public int PhotoIdProfile { get; set; }
}