using Microsoft.AspNetCore.SignalR;

namespace BlazorHomeSite.Services.SignalRLoggerProvider
{
    public class LoggingHub : Hub

    {
        public const string HubUrl = "/logging";
    }
}