using System;

namespace Graphal.Engine.Abstractions.Logging
{
    public class LogEntry
    {
        public LogEntry(LogEntryType entryType, DateTime dateTimeUtc, string text)
        {
            EntryType = entryType;
            DateTimeUtc = dateTimeUtc;
            Text = text;
        }

        public LogEntry(LogEntryType entryType, DateTime dateTimeUtc, string text, string details)
        {
            EntryType = entryType;
            DateTimeUtc = dateTimeUtc;
            Text = text;
            Details = details;
        }

        public LogEntryType EntryType { get; }

        public DateTime DateTimeUtc { get; }

        public string Text { get; }

        public string Details { get; }

        public static LogEntry InfoEntry(string text)
        {
            return new LogEntry(LogEntryType.Info, DateTime.UtcNow, text);
        }
    }
}