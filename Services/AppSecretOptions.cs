namespace BlazorHomeSite.Services
{
    //dotnet user-secrets set SendGridKey <key>
    public class AppSecretOptions
    {
        public string? SendGridKey { get; set; }
        public string? FromEmailAddress { get; set; }
    }
}