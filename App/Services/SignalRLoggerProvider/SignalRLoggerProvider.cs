using System.Collections.Concurrent;
using System.Xml.Linq;

namespace BlazorHomeSite.Services.SignalRLoggerProvider
{
    public class SignalRLoggerProvider : ILoggerProvider
    {
        private readonly SignalRLoggerConfiguration _config = new();
        private readonly ConcurrentDictionary<string, SignalRLogger> _loggers = new();

        public SignalRLoggerProvider(SignalRLoggerConfiguration config)
        {
            _config = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var logger = new SignalRLogger(categoryName, _config);
            _loggers.GetOrAdd(categoryName, logger);
            return logger;
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}