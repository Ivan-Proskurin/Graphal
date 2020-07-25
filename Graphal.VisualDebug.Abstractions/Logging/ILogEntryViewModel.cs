using System.Drawing;

namespace Graphal.VisualDebug.Abstractions.Logging
{
    public interface ILogEntryViewModel
    {
        Color ForegroundColor { get; }
        
        string Text { get; }
    }
}