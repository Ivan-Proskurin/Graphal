using System;
using System.Drawing;

using Graphal.Engine.Abstractions.Logging;
using Graphal.VisualDebug.Abstractions.Logging;

namespace Graphal.VisualDebug.ViewModels.Logging
{
    public static class LogConsoleConvertHelper
    {
        public static ILogEntryViewModel ToLogEntryViewModel(this LogEntry logEntry)
        {
            return new LogEntryViewModel
            {
                ForegroundColor = logEntry.EntryType.ToEntryLogColor(),
                Text = $"{logEntry.DateTimeUtc.ToLogEntryDateTimeString()}: {logEntry.Text}",
            };
        }

        public static Color ToEntryLogColor(this LogEntryType logEntryType)
        {
            switch (logEntryType)
            {
                case LogEntryType.Info:
                    return Color.LimeGreen;
                case LogEntryType.Warning:
                    return Color.Goldenrod;
                case LogEntryType.Error:
                    return Color.Crimson;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logEntryType), logEntryType, null);
            }
        }

        private static string ToLogEntryDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yy HH:mm:ss");
        }
    }
}