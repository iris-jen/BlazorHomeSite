using Microsoft.AspNetCore.SignalR;

namespace BlazorHomeSite.Services.SignalRLoggerProvider
{
    public class SignalRLoggerConfiguration
    {
        public int EventId { get; set; } = 0;
        public string GroupName { get; set; } = "HomeSiteLogs";
        public IHubContext<LoggingHub> HubContext { get; set; }
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
    }
}