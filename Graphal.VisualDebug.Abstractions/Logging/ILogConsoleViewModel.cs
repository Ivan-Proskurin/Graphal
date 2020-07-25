using System.Collections.ObjectModel;

namespace Graphal.VisualDebug.Abstractions.Logging
{
    public interface ILogConsoleViewModel
    {
        ObservableCollection<ILogEntryViewModel> Entries { get; }

        void Initialize();
    }
}