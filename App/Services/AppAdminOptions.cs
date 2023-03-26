namespace BlazorHomeSite.Services;

//dotnet user-secrets set SendGridKey <key>
public class AppAdminOptions
{
    public string? AdminEmailAddress { get; set; }
    public string? FromEmailAddress { get; set; }
    public string? PhotoDirectory { get; set; }
    public string? SendGridKey { get; set; }
    public bool? ShowInitAdminButton { get; set; }
}