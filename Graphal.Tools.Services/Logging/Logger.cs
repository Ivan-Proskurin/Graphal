using Graphal.Engine.Abstractions.Logging;
using Graphal.Tools.Storage.Abstractions;

namespace Graphal.Tools.Services.Logging
{
    public class Logger : ILogger, ILogObserver
    {
        private readonly ILogStorage _logStorage;

        public Logger(ILogStorage logStorage)
        {
            _logStorage = logStorage;
        }

        public void Info(string message)
        {
            var entry = LogEntry.InfoEntry(message);
            _logStorage.Append(entry);
            OnLogEntryAdded(entry);
        }

        public event LogEntryAddedEventHandler LogEntryAdded;

        private void OnLogEntryAdded(LogEntry entry)
        {
            LogEntryAdded?.Invoke(this, new LogEntryAddedEventArgs(entry));
        }
    }
}