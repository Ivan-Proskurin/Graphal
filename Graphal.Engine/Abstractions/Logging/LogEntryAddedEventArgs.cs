using System;

namespace Graphal.Engine.Abstractions.Logging
{
    public class LogEntryAddedEventArgs : EventArgs
    {
        public LogEntryAddedEventArgs(LogEntry entry)
        {
            Entry = entry;
        }

        public LogEntry Entry { get; }
    }
}