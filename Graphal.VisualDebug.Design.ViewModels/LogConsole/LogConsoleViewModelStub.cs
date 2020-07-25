using System.Collections.ObjectModel;

using Graphal.Engine.Abstractions.Logging;
using Graphal.VisualDebug.Abstractions.Logging;
using Graphal.VisualDebug.ViewModels.Logging;

namespace Graphal.VisualDebug.Design.ViewModels.LogConsole
{
    public class LogConsoleViewModelStub : ILogConsoleViewModel
    {
        public LogConsoleViewModelStub()
        {
            Entries = new ObservableCollection<ILogEntryViewModel>
            {
                new LogEntryViewModelStub(LogEntryType.Info.ToEntryLogColor(), "This is just information"),
                new LogEntryViewModelStub(LogEntryType.Warning.ToEntryLogColor(), "This is warning"),
                new LogEntryViewModelStub(LogEntryType.Error.ToEntryLogColor(), "This is error!")
            };
        }

        public ObservableCollection<ILogEntryViewModel> Entries { get; }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
    }
}