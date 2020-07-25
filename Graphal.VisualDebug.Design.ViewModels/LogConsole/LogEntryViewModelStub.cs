using System.Drawing;

using Graphal.VisualDebug.Abstractions.Logging;

namespace Graphal.VisualDebug.Design.ViewModels.LogConsole
{
    public class LogEntryViewModelStub : ILogEntryViewModel
    {
        public LogEntryViewModelStub(Color foregroundColor, string text)
        {
            ForegroundColor = foregroundColor;
            Text = text;
        }

        public Color ForegroundColor { get; }

        public string Text { get; }
    }
}