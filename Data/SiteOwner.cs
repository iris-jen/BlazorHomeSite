using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace BlazorHomeSite.Data
{
    public class SiteOwner
    {
        public int ID { get; set; }
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
    }
}