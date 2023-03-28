using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace BlazorHomeSite.Services.SignalRLoggerProvider
{
    public class SignalRLogger : ILogger
    {
        private readonly SignalRLoggerConfiguration _config;
        private readonly string _name;

        public SignalRLogger(string name, SignalRLoggerConfiguration config)
        {
            _name = name;
            _config = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _config.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                            Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (_config.EventId == 0 || _config.EventId == eventId.Id)
            {
                this._config.HubContext?.Clients.Group(_config.GroupName)
                    .SendAsync("Broadcast", "LOGGER", $"{DateTimeOffset.UtcNow:T}-UTC : {formatter(state, exception)}");
            }
        }
    }
}