namespace Services
{
    //dotnet user-secrets set SendGridKey <key>
    public class AppAdminOptions
    {
        public string? SendGridKey { get; set; }
        public string? FromEmailAddress { get; set; }
        public string? AdminEmailAddress { get; set; }
        public bool? ShowInitAdminButton { get; set; }
        public string? PhotoDirectory { get; set; }
    }
}