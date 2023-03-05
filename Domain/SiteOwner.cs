using Domain.Photos;

namespace Domain
{
    public class SiteOwner
    {
        public int ID { get; set; } = -1;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Introduction { get; set; }
        public string? Location { get; set; }
        public string? About { get; set; }
        public string? DiscordUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstaUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public string? LinkdinUrl { get; set; }
        public Photo? ProfilePhoto { get; set; }
        public Photo? HomePageBackground { get; set; }
        public bool EnablePhotosFeature { get; set; }
        public bool EnableMusicFeature { get; set; }
        public bool EnableBlogFeature { get; set; }
    }
}