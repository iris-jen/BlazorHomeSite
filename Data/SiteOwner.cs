using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace BlazorHomeSite.Data
{
    public class SiteOwner : IdentityUser
    {
        public string? Introduction { get; set; }
        public string? About { get; set; }
        public Photo? ProfilePhoto { get; set; }
        public Photo? HomePageBackground { get; set; }
    }
}