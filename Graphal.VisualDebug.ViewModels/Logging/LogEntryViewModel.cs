using System.Drawing;

using Graphal.VisualDebug.Abstractions.Logging;

namespace Graphal.VisualDebug.ViewModels.Logging
{
    public class LogEntryViewModel : ILogEntryViewModel
    {
        public Color ForegroundColor { get; internal set; }

        public string Text { get; internal set; }
    }
}